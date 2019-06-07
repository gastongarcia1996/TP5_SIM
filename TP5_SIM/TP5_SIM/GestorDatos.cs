using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP5_SIM
{
    class GestorDatos
    {
        private readonly double detenida = 0;
        private readonly double funcionando = 1;

        private readonly double eventoInicial = -1;
        private readonly double llegadaFamilia = 1;
        private readonly double finDeCompra = 2;
        private readonly double finDeSubidaCalecita = 3;
        private readonly double rompeEnLlanto = 4;

        private readonly double estadoBoleteriaLibre = 0;
        private readonly double estadoBoleteriaOcupado = 1;

        private readonly double siTieneFicha = 1;
        private readonly double noTieneFicha = 0;

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

            double evento = 0;
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
            double estadoCalecita = 0.0; 
            double colaCalecita = 8;
            double ProxVueltaCalecita = 0.0;
            double estadoBoleteria = this.estadoBoleteriaLibre;
            double colaBoleteria = 0;
            double tiempoRompeEnLlanto = 0.0;
            double taParaLlorar = -1;
            double proximoRompeEnLlanto = -1;
            int cont = 0;
            int nroExp = 1;
            double menorTiempoEvento = 0.0;

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
                    rndCantNiños = GenerarRandom();
                    cantNiños = CalcularCantNiños(1, 5, rndCantNiños);
                    rndFinDeCompra = GenerarRandom();
                    tiempoFinDeCompra = CalcularTiempoFinDeCompra(1, 2, rndFinDeCompra);
                    tiempoFinSubidaCal = 0;
                    tiempoRompeEnLlanto = (evento == finDeSubidaCalecita) ? reloj : tiempoRompeEnLlanto;
                    taParaLlorar = (evento == tiempoFinSubidaCal) ? 
                    estadoCalecita = (colaCalecita == 0 || evento == finDeSubidaCalecita || ) ? this.funcionando : this.detenida;
                    colaCalecita = CalculoColaCalecita(colaCalecita, cantNiños, estadoCalecita);
                    ProxVueltaCalecita = CalculoProxVueltaCalecita(evento, reloj, ProxVueltaCalecita);
                    estadoBoleteria = CalculoEstadoBoleteria(evento, tieneFichas, estadoBoleteria);
                    colaBoleteria = AgregarFamiliaColaBoleteria(evento, estadoBoleteria, colaBoleteria);

                }
            }

            this.matrizDatos = datos;
        }
       

        private double AgregarFamiliaColaBoleteria(double eventoActual, double estadoBoleteriaActual, double colaBoleteria)
        {
            if (estadoBoleteriaActual == estadoBoleteriaOcupado && eventoActual == llegadaFamilia)
            {
                return colaBoleteria + 1;
            }
            return colaBoleteria;
        }

        private double CalculoEstadoBoleteria(double evento, double tieneFicha, double estadoBoleteriaActual)
        {
            if (evento == this.llegadaFamilia && tieneFicha == siTieneFicha)
            {
                if (estadoBoleteriaActual == estadoBoleteriaLibre) return estadoBoleteriaOcupado;
            }
            return estadoBoleteriaOcupado;
        }

        private double CalculoProxVueltaCalecita(double evento, double reloj, double proximaVuelta)
        {
            double auxiliar = 0.0;

            if (evento == this.finDeSubidaCalecita)
            {
                auxiliar = reloj + 4.0;
            }

            if (evento == rompeEnLlanto)
            {
                auxiliar = proximaVuelta + 0.3;
            }
            return auxiliar;
        }

        private double CalculoColaCalecita(double colaCalecitaActual, double cantNiños, double estadoCalecita)
        {
            if (estadoCalecita == this.detenida)
            {
                return colaCalecitaActual + cantNiños;
            }
            return colaCalecitaActual;
        }

        private double CalcularTiempoFinDeCompra(int a, int b, double rnd)
        {
            return a + (rnd * (b - a));
        }

        private double CalcularCantNiños(int a, int b, double rnd)
        {
            return Math.Truncate(a + (rnd * (b - a)));
        }

        private double TieneFichas(double rnd)
        {
            return (rnd < 0.3) ? siTieneFicha : noTieneFicha;
        }

        private double GenerarRandom()
        {
            return Math.Truncate((double)(random.Next() * 10000) / 10000);
        }

        private double TiempoLlegadaFamilia()
        {
            return Math.Truncate((double)((-3 * Math.Log(1 - GenerarRandom())) * 10000) / 10000);
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
