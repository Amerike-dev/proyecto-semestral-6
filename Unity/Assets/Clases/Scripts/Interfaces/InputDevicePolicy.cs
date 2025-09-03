using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public static class InputDevicePolicy
{
    // CONFIG
    const int MIN_GAMEPADS = 3;           // necesitas 3 controles minimo
    const string SCHEME_KBM = "KeyboardMouse";
    const string SCHEME_PAD = "Gamepad";

    // Estado
    static PlayerInput keyboardOwner;               // dueno exclusivo del teclado/mouse
    static readonly HashSet<Gamepad> claimedPads = new HashSet<Gamepad>(); // pads usados

    // --- API: Llama esto en Awake() del PlayerController ---
    public static void Assign(PlayerInput pi)
    {
        if (!IsValid(pi)) return;

        UnpairIfPresent(Touchscreen.current, pi);

        // Modo "solo teclado": menos de 3 gamepads => solo habilitamos al dueno del teclado
        if (Gamepad.all.Count < MIN_GAMEPADS)
        {
            // Si este Player es/sera el dueno del teclado, quedatelo; si no, desactivate
            bool wantsKeyboard = IsKBMJoin(pi);
            if (keyboardOwner == null && wantsKeyboard)
            {
                keyboardOwner = pi;
                pi.neverAutoSwitchControlSchemes = false;
                PairIfPresent(Keyboard.current, pi);
                PairIfPresent(Mouse.current, pi);
                ForceScheme(pi, SCHEME_KBM, Only(Keyboard.current, Mouse.current));
            }
            else
            {
                // No hay suficientes mandos => no intentes nada (Quitar Warnings)
                SafeDisable(pi);
            }
            return; // fin sin politicas
        }

        // Desde aqui: tenemos >= 3 mandos => aplica la politica completa
        // Quitar schemes Como Touch
        NormalizeScheme(pi);

        bool kbmJoin = IsKBMJoin(pi);

        if (kbmJoin)
        {
            // Si ya hay 4 pads reclamados, el teclado no toma nada extra
            if (CountClaimedPads() >= 4)
            {
                UnpairIfPresent(Keyboard.current, pi);
                UnpairIfPresent(Mouse.current, pi);
                pi.neverAutoSwitchControlSchemes = true;
                ForceGamepadIfHasOne(pi);
                return;
            }

            if (keyboardOwner == null)
            {
                keyboardOwner = pi;
                pi.neverAutoSwitchControlSchemes = false;
                PairIfPresent(Keyboard.current, pi);
                PairIfPresent(Mouse.current, pi);
                ForceScheme(pi, SCHEME_KBM, Only(Keyboard.current, Mouse.current));

                // Marca pads que ya tenga el owner
                if (IsUserValid(pi))
                    foreach (var d in pi.user.pairedDevices)
                        if (d is Gamepad g) claimedPads.Add(g);
            }
            else
            {
                // Ya hay dueno => este jugador NO usa teclado
                UnpairIfPresent(Keyboard.current, pi);
                UnpairIfPresent(Mouse.current, pi);
                pi.neverAutoSwitchControlSchemes = true;
                ForceGamepadIfHasOne(pi);
            }
        }
        else
        {
            // Entro con mando => solo su mando
            pi.neverAutoSwitchControlSchemes = true;
            UnpairIfPresent(Keyboard.current, pi);
            UnpairIfPresent(Mouse.current, pi);

            if (IsUserValid(pi))
                foreach (var d in pi.user.pairedDevices)
                    if (d is Gamepad g) claimedPads.Add(g);

            ForceGamepadIfHasOne(pi);
        }
    }

    // --- API: PlayerController del dueno del teclado (si quiere usar un pad libre) ---
    public static void TryAdoptFreePadIfKeyboardOwner(PlayerInput pi)
    {
        if (!IsValid(pi) || pi != keyboardOwner) return;
        if (CountClaimedPads() >= 4) return;

        foreach (var g in Gamepad.all)
        {
            if (claimedPads.Contains(g)) continue;
            if (!AnyButtonDown(g)) continue;

            PairIfPresent(g, pi);
            claimedPads.Add(g);
            // El dueno tiene auto-switch
            return;
        }
    }

    // ---------- helpers ----------
    static bool IsValid(PlayerInput pi) => pi != null && pi.isActiveAndEnabled;
    static bool IsUserValid(PlayerInput pi) => IsValid(pi) && pi.user.valid;

    static int CountClaimedPads() => claimedPads.Count;

    static bool IsKBMJoin(PlayerInput pi)
    {
        if (!IsValid(pi)) return false;
        return pi.currentControlScheme == SCHEME_KBM
               || HasDevice(pi, Keyboard.current)
               || HasDevice(pi, Mouse.current);
    }

    static bool HasDevice(PlayerInput pi, InputDevice d) =>
        d != null && (pi.devices.Contains(d) || (IsUserValid(pi) && pi.user.pairedDevices.Contains(d)));

    static void PairIfPresent(InputDevice d, PlayerInput pi)
    {
        if (d == null || !IsValid(pi)) return;
        if (IsUserValid(pi) && !pi.user.pairedDevices.Contains(d))
            InputUser.PerformPairingWithDevice(d, pi.user);
    }

    static void UnpairIfPresent(InputDevice d, PlayerInput pi)
    {
        if (d == null || !IsValid(pi) || !IsUserValid(pi)) return;
        if (pi.user.pairedDevices.Contains(d))
            pi.user.UnpairDevice(d);
    }

    static void ForceGamepadIfHasOne(PlayerInput pi)
    {
        if (!IsUserValid(pi)) return;
        var devices = pi.user.pairedDevices.ToArray();
        if (!devices.Any(d => d is Gamepad)) return;
        ForceScheme(pi, SCHEME_PAD, devices);
    }

    static void ForceScheme(PlayerInput pi, string scheme, InputDevice[] devices)
    {
        try { pi.SwitchCurrentControlScheme(scheme, devices); } catch { }
    }

    static InputDevice[] Only(params InputDevice[] devices) =>
        devices.Where(d => d != null).Cast<InputDevice>().ToArray();

    static void SafeDisable(PlayerInput pi)
    {
        // Desactiva la entrada para evitar warnings. Es Reactivado cuando conecten los pads
        try { pi.DeactivateInput(); } catch { }
        pi.enabled = false; // Podemos comentar esta linea / es opcional
    }

    static void NormalizeScheme(PlayerInput pi)
    {
        // Si el scheme actual no es KBM ni Gamepad, se corrige
        string s = pi.currentControlScheme;
        if (s == SCHEME_KBM || s == SCHEME_PAD) return;

        UnpairIfPresent(Touchscreen.current, pi);

        if (HasAnyPairedPad(pi))
            ForceGamepadIfHasOne(pi);
        else if (HasDevice(pi, Keyboard.current) || HasDevice(pi, Mouse.current))
            ForceScheme(pi, SCHEME_KBM, Only(Keyboard.current, Mouse.current));
    }

    static bool HasAnyPairedPad(PlayerInput pi) =>
        IsUserValid(pi) && pi.user.pairedDevices.Any(d => d is Gamepad);

    static bool AnyButtonDown(Gamepad gp)
    {
        if (gp == null) return false;
        return gp.buttonSouth.wasPressedThisFrame || gp.buttonNorth.wasPressedThisFrame ||
               gp.buttonEast.wasPressedThisFrame || gp.buttonWest.wasPressedThisFrame ||
               gp.startButton.wasPressedThisFrame || gp.selectButton.wasPressedThisFrame ||
               gp.leftShoulder.wasPressedThisFrame || gp.rightShoulder.wasPressedThisFrame ||
               gp.leftStickButton.wasPressedThisFrame || gp.rightStickButton.wasPressedThisFrame ||
               gp.dpad.up.wasPressedThisFrame || gp.dpad.down.wasPressedThisFrame ||
               gp.dpad.left.wasPressedThisFrame || gp.dpad.right.wasPressedThisFrame;
    }
}