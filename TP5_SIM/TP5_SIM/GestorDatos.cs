using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP5_SIM
{
    class GestorDatos
    {
        private double[,] matrizDatos;
        private Random random = new Random();

        public GestorDatos()
        {
            matrizDatos = new double[100, 26];
        }

        public void CargarDatos(int cantSimulaciones, int desde)
        {
            int hasta = 0;
            double[,] datos = new double[101, 26];

            double Estado = 0;
            double reloj = 0.0;
            double rndTiempoLlegada = 0.0;
            double tiempoLlegada = 0.0;
            double proxLlegada = 0.0;
            double rndTieneFichas = 0.0;
            double tieneFichas = -1;
            double rndCantNiños = 0.0;
            double cantNiños = 0.0;
            double rndFinDeCompra = 0.0;
            double tiempoFinDeCompra = 0.0;
            double tiempoFinSubidaCal = 0.0;
            double RompeEnLlandoTiempo = 0.0;
            double ProxRompeEnLlanto = 0.0;
            double EstadoCalecita = 0.0;
            double ColaCalecita = 0.0;
            double ProxVueltaCalecita = 0.0;
            double EstadoBoleteria = 0;
            double ColaBoleteria = 0;
            int cont = 0;
            int nroExp = 1;

            for (int i = 0; i <= cantSimulaciones; i++)
            {
                if (nroExp >= desde && cont <= hasta)
                {
                    if ((cantSimulaciones - desde) < 100)
                    {
                        hasta = cantSimulaciones - desde;
                    }

                    datos[cont, 0] = 0;

                    cont++;
                }

                if (i != cantSimulaciones)
                {
                    reloj = reloj + proxLlegada;
                    rndTiempoLlegada = GenerarRandom();
                    tiempoLlegada = CalcularTiempoLlegada(1, 5, rndTiempoLlegada);
                    proxLlegada = reloj + tiempoLlegada;
                    rndTieneFichas = GenerarRandom();
                    tieneFichas = TieneFichas(rndTieneFichas);
                }
            }

            this.matrizDatos = datos;
        }

        private int TieneFichas(double rnd)
        {
            return (rnd < 0.3) ? 1 : 0;
        }

        public int CalcularEvento()
        {
            if ()
        }

        private double GenerarRandom()
        {
            return Math.Truncate((double)(random.Next() * 10000) / 10000);
        }

        private double CalcularTiempoLlegada(double a, double b, double rnd)
        {
            return a + (rnd * (b - a));
        }
        public double[,] GetDatos()
        {
            return matrizDatos;
        }
    }
}
