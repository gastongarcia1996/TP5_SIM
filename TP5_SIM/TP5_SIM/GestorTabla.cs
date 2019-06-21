using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TP5_SIM
{
    class GestorTabla
    {
        private DataGridView tabla = null;

        public GestorTabla(DataGridView tabla)
        {
            this.tabla = tabla;
        }

        public void CompletarTabla2(double[,] datos)
        {
            tabla.Rows.Clear();

            int fila = 0;
            for (int i = 0; i < datos.GetLength(0); i++)
            {
                if (datos[i, 0] == -1)
                {
                    tabla.Rows[fila].Cells[0].Value = "-";
                    tabla.Rows[fila].Cells[1].Value = "-";
                    tabla.Rows[fila].Cells[2].Value = "-";
                }
                else
                {
                    tabla.Rows[fila].Cells[0].Value = datos[i, 0];
                    tabla.Rows[fila].Cells[1].Value = datos[i, 1];
                    tabla.Rows[fila].Cells[2].Value = datos[i, 2];
                }
                fila++;
            }
        }
        public void CompletarTabla(double[,] datos)
        {
            tabla.Rows.Clear();

            int fila = 0;
            for (uint i = 0; i < datos.GetLength(0); i++)
            {
                if (datos[i, 0] == 0) break;

                tabla.Rows.Add();
                tabla.Rows[fila].Cells[0].Value = StringEvento(datos[i, 1]);
                tabla.Rows[fila].Cells[1].Value = CadenaAuxiliarTabla(datos[i, 2]);
                tabla.Rows[fila].Cells[2].Value = CadenaAuxiliarTabla(datos[i, 3]);
                tabla.Rows[fila].Cells[3].Value = CadenaAuxiliarTabla(datos[i, 4]);
                tabla.Rows[fila].Cells[4].Value = CadenaAuxiliarTabla(datos[i, 5]);
                tabla.Rows[fila].Cells[5].Value = CadenaAuxiliarTabla(datos[i, 6]);
                tabla.Rows[fila].Cells[6].Value = StringTieneFicha(datos[i, 7]);
                tabla.Rows[fila].Cells[7].Value = CadenaAuxiliarTabla(datos[i, 8]);
                tabla.Rows[fila].Cells[8].Value = CadenaAuxiliarTabla(datos[i, 9]);
                tabla.Rows[fila].Cells[9].Value = CadenaAuxiliarTabla(datos[i, 10]);
                tabla.Rows[fila].Cells[10].Value = CadenaAuxiliarTabla(datos[i, 11]);
                tabla.Rows[fila].Cells[11].Value = CadenaAuxiliarTabla(datos[i, 12]);
                tabla.Rows[fila].Cells[12].Value = CadenaAuxiliarTabla(datos[i, 13]);
                tabla.Rows[fila].Cells[13].Value = CadenaAuxiliarTabla(datos[i, 14]);
                tabla.Rows[fila].Cells[14].Value = CadenaAuxiliarTabla(datos[i, 15]);
                tabla.Rows[fila].Cells[15].Value = CadenaAuxiliarTabla(datos[i, 16]);
                tabla.Rows[fila].Cells[16].Value = StringEstadoCalecita(datos[i, 17]);
                tabla.Rows[fila].Cells[17].Value = CadenaAuxiliarTabla(datos[i, 18]);
                tabla.Rows[fila].Cells[18].Value = CadenaAuxiliarTabla(datos[i, 19]);
                tabla.Rows[fila].Cells[19].Value = StringEstadoBoleteria(datos[i, 20]);
                tabla.Rows[fila].Cells[20].Value = StringColaBoleteria(datos[i, 21]);
                tabla.Rows[fila].Cells[21].Value = CadenaAuxiliarTabla(datos[i, 22]);
                tabla.Rows[fila].Cells[22].Value = CadenaAuxiliarTabla(datos[i, 23]);
                tabla.Rows[fila].Cells[23].Value = CadenaAuxiliarTabla(datos[i, 24]);
                tabla.Rows[fila].Cells[24].Value = CadenaAuxiliarTabla(datos[i, 25]);

                fila++;
            }
        }

        private string StringColaBoleteria(double colaBoleteria)
        {
            if (colaBoleteria == -1) return "0";

            return colaBoleteria.ToString();
        }

        private string StringEstadoBoleteria(double estadoBoleteria)
        {
            if (estadoBoleteria == 0) return "Libre";
            else return "Ocupado";
        }

        private string StringEstadoCalecita(double estado)
        {
            if (estado == 0) return "Detenida";
            else return "Funcionando";
        }

        private string StringTieneFicha(double tieneFicha)
        {
            if (tieneFicha == -1) return "-";
            else if (tieneFicha == 0) return "NO";
            else return "SI";
        }

        private string StringEvento(double evento)
        {
            if (evento == -1) return "Evento inicial";
            else if (evento == 1) return "Llegada familia";
            else if (evento == 2) return "Fin de compra";
            else if (evento == 3) return "Fin subida calecita";
            else if (evento == 4) return "Fin de vuelta calecita";
            else return "Rompe en llanto";
        }
        private string CadenaAuxiliarTabla(double dato)
        {
            if (dato.ToString().Equals("-1"))
            {
                return "-";
            }
            return dato.ToString();
        }
    }
}

