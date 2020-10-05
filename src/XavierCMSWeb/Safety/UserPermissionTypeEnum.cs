// GNU Version 3 License Copyright (c) 2020 Javier Cañon | https://javiercanon.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
namespace XavierCMSWeb.Safety
{

    /// <summary>
    /// Enum for control users permissions. Needs to be synchronized with db ids.
    /// Enum value = db id (UserPerTypeId).
    /// </summary>
    public enum UserPermissionTypeEnum
    {
        /// <summary>
        /// For non valid Enum when search for enum string value 
        /// </summary> 
        NoValidEnum = -1,


        // PERMISSIONS ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO USUARIOS
        /// </summary>
        BINUMBER_ACCESO_USUARIOS = 1,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO ROLES
        /// </summary>
        BINUMBER_ACCESO_ROLES = 2,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO DASHBOARD DIARIO
        /// </summary>
        BINUMBER_ACCESO_DASHBOARD_DIARIO = 3,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO DASHBOARD EST NUM APOSTADOS
        /// </summary>
        BINUMBER_ACCESO_DASHBOARD_EST_NUM_APOSTADOS = 4,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO DASHBOARD PREMIACION
        /// </summary>
        BINUMBER_ACCESO_DASHBOARD_PREMIACION = 5,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO GRID PREMIOS
        /// </summary>
        BINUMBER_ACCESO_GRID_PREMIOS = 20,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO CONSULTAS PREGENERADAS
        /// </summary>
        BINUMBER_ACCESO_CONSULTAS_PREGENERADAS = 21,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO CONSULTAS DIARIAS
        /// </summary>
        BINUMBER_ACCESO_CONSULTAS_DIARIAS = 22,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO DISTRIBUCION NORMAL VENTAS
        /// </summary>
        BINUMBER_ACCESO_DISTRIBUCION_NORMAL_VENTAS = 23,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO DISTRIBUCION NORMAL PREMIOS
        /// </summary>
        BINUMBER_ACCESO_DISTRIBUCION_NORMAL_PREMIOS = 24,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO ESTADISTICAS POR NUMERO
        /// </summary>
        BINUMBER_ACCESO_ESTADISTICAS_POR_NUMERO = 25,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO ESTADISTICAS RESULTADOS
        /// </summary>
        BINUMBER_ACCESO_ESTADISTICAS_RESULTADOS = 26,

        /// <summary>
        /// PERMISSION FOR CAT.(1) BINUMBER, ACCESO PREMIOS RANGOS
        /// </summary>
        BINUMBER_ACCESO_PREMIOS_RANGOS = 27,



    }


}