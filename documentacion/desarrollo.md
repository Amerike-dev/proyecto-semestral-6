## Desarrollo
Hay reglas que se deben de seguir para llevar un buen desarrollo en conjunto.

1. Antes de hacer un pull request, asegurate que tu rama esta actualizada desde main.
 - `git switch main`    cambia a la rama main
 - `git pull`           actualiza la rama
 - `git checkout -b <nombre de la rama>` crea una nueva rama a partir de main actualizada

2. No debes hacer un `merge` a main hasta que tengas la aprobacion del profesor.
    - Si se hace un merge a main sin approve no se contaran los `story points`.
    - Actualiza el ticket con Done una vez mergeado.
    - Si te marcan `conflictos` actualiza main (pull) y luego, cambiate a tu rama y mergea main localmente.
    `git switch main`
    `git pull`
    `git switch <nombre de la rama>`
    `git merge main`

3. No borres ninguna rama que no sea tuya.

4. Al hacer merge, borra tu rama.

5. Se responsable, debes de probar tus cambions antes de mergear, asegurate que tu desarrollo no este destruyendo el trabajo de los demas. 