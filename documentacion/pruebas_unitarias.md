## Pruebas unitarias
En un proyecto de calidad, se deben agregar pruebas unitarias, no demasiadas pero tampoco ninguna, las pruebas unitarias son simulaciones que pueden presentarse, por lo general se piensan en situaciones inesperadas y esperadas y estar preparados para cualquier cosa.

## Configuracion
Hay dos tipos de pruebas unitarias dentro de unity

### Editor Tests
Pruebas que solamente se enfocan en el codigo y como se espera que se comporte

### Run Tests
Pruebas que se ejecutan runtime, es decir, mientras se ejecuta el juego.

Nos enfocaremos en las pruebas `Editor`.

## Creacion de pruebas a nivel Editor
Para poder realizar las pruebas se debe seguir los siguientes pasos.

1. Tener una carpeta en `Assets/Tests`
2. Tener un `Tests Assembly Folder`, se puede crear con click derecho en la opcion `Testing`.
3. Unity maneja las pruebas separadas del codigo, por lo que si queremos hacer referencia a nuestro codigo, tenemos que crear un `Assembly Definition`.
4. Posicionate en la carpeta que quieras que contenga la referencia a probar. Es decir, si quieres probar los modelos, como `Player.cs`, `Enemy.cs`, `Object.cs` entonces, ponte en la carpeta `../models`.
5. Crea un nuevo `Assembly Definition` con click derecho `Create/Scripting`.
6. Nombra el archivo con el nombre de la carpeta, por ejemplo si estas en `Models` nombra el archivo como `Models.Core`.
7. IMPORTANTE Si ya existe un Assembly Definition en una  carpeta superior a la que quieres usar, no es necesario crear otro.
8. Regresa a tu carpeta `Tests/Editor`
9. Haz click en el archivo que se creo, que tiene extension assembly.
10. Se abrira una ventana en el inspector.
11. En la seccion `Assembly Definition References` haz click en la flecha de `+`.
Selecciona la referencia que acabas de hacer, en el ejemplo anterior era `Models.Core` pero agrega la que tu estas teniendo.
12. En `Tests/Editor` Crea una nueva clase que no herede de monobehavior, ponle de nombre lo que quieres probar, si quieres probar a `Player` ponle `PlayerTests.cs`.
13. Abre el nuevo archivo y empieza a hacer las pruebas.

## Estructura de las pruebas
Cada prueba se conforma por 3 pasos

1. Arrange
2. Act
3. Assert

### Arrange
Es el arreglo o estructuracion de la prueba, crear lo que necesitas antes de ejecutar la prueba. Por ejemplo, si vaz a probar que un player sea golpeado por un enemigo, entonces, necesitas crear al enemigo antes.

### Act
Es la accion de la prueba, por lo general es una linea, siguiendo el ejemplo anterior, quiero probar una funcion del Player de recibir un golpe. Por ejemplo, puede que tenga una funcion `damage()` y esta le reste vida. Act seria cuando se llama esta funcion.

### Assert
La etapa mas importante, se usan funciones para comprobar los resultados. Si tengo un player de 10 de vida y un enemigo lo ataca con 5, yo estoy esperando a que mi player tenga 5 de vida. Entonces seria `Assert` que mi `Player.health` es igual a 5.

**NOTA**
Es importante saber que no siempre existen los tres elementos, minimo existe el assert y el act.

## Escribiendo pruebas
C# usa una notacion especifica de las pruebas, considera el siguiente ejemplo:

Clase a probar:

```C#
public class Player
{
    public int Health { get; private set; }
    public int Damage { get; private set; }

    public Player(int startingHealth, int damage)
    {
        Health = startingHealth;
        Damage = damage;
    }

    public void TakeDamage(int amount)
    {
        if (amount > 0)
        {
            Health -= amount;
            if (Health < 0)
            {
                Health = 0;
            }
        }
    }
}
```

En esta clase, hay un consturctor y una funcion `TakeDamage()`

Quiero realizar las siguientes pruebas:

1. Que la funcion `TakeDamage()` funciona correctamente.
2. Que el constructor `Player()` me esta generando un objeto correcto.

Para esto, usamos la libreria `NUnit.Framework` y se debe de definir explicitamente que nuestra clase es una clase de prueba. Ademas, tendremos funciones en nuestra clase de prueba que seran los tests.

Cada test, a diferencia de una funcion normal, debe estar separada por guiones bajos.

```C#
using NUnit.Framework;

[TestFixture]
public class PlayerTests
{
    [Test]
    public void Player_TakeDamage_ReducesHealth()
    {
        // 1. Arrange: Configura el entorno de la prueba
        var player = new Player(100, 10);
        int damageAmount = 25;

        // 2. Act: Ejecuta el método que quieres probar
        player.TakeDamage(damageAmount);

        // 3. Assert: Verifica que el resultado es el esperado
        // NUnit proporciona varios métodos Assert
        Assert.AreEqual(75, player.Health);
    }

    [Test]
    public void Player_Constructor_IsInstanceOfPlayer()
    {
        // Act
        Player player = new Player(10, 10);

        // Assert
        Assert.IsInstanceOf<Player>(player);
    }



    [Test]
    public void Player_TakeDamage_DoesNotGoBelowZeroHealth()
    {
        // 1. Arrange
        var player = new Player(10, 5);
        int damageAmount = 20;

        // 2. Act
        player.TakeDamage(damageAmount);

        // 3. Assert
        Assert.AreEqual(0, player.Health);
    }
}
```

Como puedes ver, en la primer prueba reviso que se esta restando vida.

En la segunda prueba reviso que el objeto creado es instancia de la clase `Player`

Que estaria haciendo la tercer prueba?

### Descripcion de pruebas
Las pruebas son describidas por la funcion, no importa si es muy largo el nombre de la funcion. La estructura que manejan son:

`{Clase a probar}_{Funcion a probar}_{Condicion Esperada}`

## Clase Assert
La clase Assert tiene muchas funciones que ayudan a lograr el objetivo.

Algunos comunes son:

```C#
Assert.IsFalse(Boolean)
Assert.IsInstanceOf(Type, Object)
Assert.IsNotNull(Object)
Assert.That(Boolean, String)
Assert.That(Object, IResolveConstraint)
Assert.That(TestDelegate, IResolveConstraint)
...
```

Ademas de los clasicos, tambien hay asserts mas complejos. Revisa la documentacion para saber cual necesitas usar.


Clase Assert
https://learn.microsoft.com/es-es/dotnet/api/nunit.framework.assert?view=xamarin-ios-sdk-12


Otras funciones
https://docs.nunit.org/articles/nunit/writing-tests/assertions/assertions.html