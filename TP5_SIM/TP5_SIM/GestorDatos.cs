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
        private readonly double finDeVueltaCalecita = 4;
        private readonly double rompeEnLlanto = 5;

        private readonly double estadoBoleteriaLibre = 0;
        private readonly double estadoBoleteriaOcupado = 1;

        private readonly double siTieneFicha = 1;
        private readonly double noTieneFicha = 0;

        private readonly double subiendo = 0;
        private readonly double enCalecita = 1;
        private readonly double enColaCompra = 2;
        private readonly double enColaCalecita = 3;

        private double acumuladorTiempoSubida = 0;

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
            LinkedList<Niño> listaAuxiliarNiños = new LinkedList<Niño>();
            GestorNiños gestorNiños = new GestorNiños(listaAuxiliarNiños);

            
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
            double tiempoRompeEnLlanto = 0.0;
            double taParaLlorar = -1;
            double proximoRompeEnLlanto = -1;
            double estadoCalecita = 0.0; 
            double colaCalecita = 8;
            double proxVueltaCalecita = 0.0;
            double estadoBoleteria = this.estadoBoleteriaLibre;
            double colaBoleteria = 0;
            double lugaresVacios = 0;
            double cantidadFichasNoCompradas = 0;
            double cantidadFichasCompradas = 0;
            
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
                    evento = SetEvento(proxLlegada, tiempoFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxVueltaCalecita);
                    reloj = CalculoReloj(proxLlegada, tiempoFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxVueltaCalecita);
                    rndTiempoLlegada = GenerarRandom();
                    tiempoLlegada = CalcularTiempoLlegada(1, 5, rndTiempoLlegada);
                    proxLlegada = reloj + tiempoLlegada;
                    rndTieneFichas = GenerarRandom();
                    tieneFichas = TieneFichas(rndTieneFichas);
                    rndCantNiños = GenerarRandom();
                    cantNiños = CalcularCantNiños(1, 5, rndCantNiños);
                    rndFinDeCompra = GenerarRandom();
                    tiempoFinDeCompra = CalcularTiempoFinDeCompra(1, 2, rndFinDeCompra);
                    tiempoFinSubidaCal = CalcularTiempoSubidaCalecita(listaAuxiliarNiños);
                    tiempoRompeEnLlanto = (evento == finDeSubidaCalecita) ? reloj : tiempoRompeEnLlanto;
                    taParaLlorar = CalculoTaParaLlorar(evento, taParaLlorar, reloj);
                    proximoRompeEnLlanto = tiempoRompeEnLlanto + reloj;
                    estadoCalecita = ((colaCalecita == 0 || evento == finDeSubidaCalecita) && evento != rompeEnLlanto) ? this.funcionando : this.detenida;
                    colaCalecita = CalculoColaCalecita(colaCalecita, cantNiños, estadoCalecita);
                    proxVueltaCalecita = CalculoProxVueltaCalecita(evento, reloj, proxVueltaCalecita);
                    estadoBoleteria = CalculoEstadoBoleteria(evento, tieneFichas, estadoBoleteria);
                    colaBoleteria = AgregarFamiliaColaBoleteria(evento, estadoBoleteria, colaBoleteria);
                    lugaresVacios = CalculoLugaresVacios(estadoCalecita, evento, colaCalecita);
                    cantidadFichasNoCompradas = (evento == llegadaFamilia && tieneFichas == siTieneFicha) ? cantidadFichasNoCompradas += cantNiños : cantidadFichasNoCompradas;
                    cantidadFichasCompradas = (evento == llegadaFamilia && tieneFichas == noTieneFicha) ? cantidadFichasCompradas += cantNiños : cantidadFichasCompradas;

                    GestorNiño(evento, tieneFichas, estadoCalecita, colaCalecita, listaAuxiliarNiños, cantNiños);
                }
            }

            this.matrizDatos = datos;
        }
       
        private double CalculoReloj(double proximaLlegada, double tiempoFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxVueltaCalecita)
        {
            double menor = proximaLlegada;
            if (menor > tiempoFinDeCompra && tiempoFinDeCompra != -1) menor = tiempoFinDeCompra;
            else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            else if (menor > proxVueltaCalecita && proxVueltaCalecita != -1) menor = proxVueltaCalecita;

            return menor;
        }
        private double SetEvento(double proximaLlegada, double tiempoFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxVueltaCalecita)
        {
            double menor = proximaLlegada;
            if (menor > tiempoFinDeCompra && tiempoFinDeCompra != -1) menor = tiempoFinDeCompra;
            else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            else if (menor > proxVueltaCalecita && proxVueltaCalecita != -1) menor = proxVueltaCalecita;

            if (menor == proximaLlegada) return llegadaFamilia;
            else if (menor == tiempoFinDeCompra) return finDeCompra;
            else if (menor == tiempoFinSubidaCal) return tiempoFinSubidaCal;
            else if (menor == proximoRompeEnLlanto) return rompeEnLlanto;
            else return finDeVueltaCalecita;
        }
        private double CalcularTiempoSubidaCalecita(LinkedList<Niño> listaNiños)
        {
            double acu = 0;
            if (listaNiños.Count == 0) return acu;
            else
            {                
                foreach (Niño n in listaNiños)
                {
                    acu += (n.GetTiempoSubida() != -1) ? n.GetTiempoSubida() : 0;
                }
            }
            return acu;
        }
        private void GestorNiño(double evento, double tieneFicha, double estadoCalecita, double colaCalecita, LinkedList<Niño> lista, double cantidadNiñosLlegan)
        {

            if (cantidadNiñosLlegan < 0) return;
            double rnd = GenerarRandom();
            double tiempoSubida = 0.0;
            if (evento == llegadaFamilia && tieneFicha == siTieneFicha)
            {
                tiempoSubida = CalcularTiempoSubida(8, 18, rnd);
                this.acumuladorTiempoSubida += tiempoSubida;
                lista.AddFirst(new Niño(subiendo, rnd, tiempoSubida));
            }
            if (evento == llegadaFamilia && tieneFicha == noTieneFicha)
            {
                lista.AddFirst(new Niño(enColaCompra, -1, -1));
            }
            if (evento == finDeSubidaCalecita)
            {
                foreach (Niño n in lista)
                {
                    n.setEstado(enCalecita);
                    n.setRnd(-1);
                    n.setTiempoSubida(-1);
                }
            }
            if (evento == finDeVueltaCalecita)
            {
                foreach (Niño n in lista)
                {
                    if (n.GetEstado() == enCalecita) lista.Remove(n);
                }
            }
            if (evento == llegadaFamilia && tieneFicha == siTieneFicha && estadoCalecita == funcionando)
            {
                lista.AddFirst(new Niño(enColaCalecita, -1, -1));
            }
        }

        private double CalcularTiempoSubida(double a, double b, double rnd)
        {
            return Math.Truncate(a + (rnd * (b - a)));
        }

        private double CalculoTaParaLlorar(double evento, double taParaLlorarActual, double reloj)
        {
            double ret = 0.0;
            if (taParaLlorarActual == -1 && evento == finDeSubidaCalecita)
            {
                ret = 4.5;
            }
            else 
            {
                if (evento == rompeEnLlanto) ret = 4.5;
                else
                {
                    if (evento == finDeVueltaCalecita) ret = taParaLlorarActual - 4.0;
                }
            }

            return ret;
        }
        private double CalculoLugaresVacios(double estadoCalecita, double evento, double colaCalecita)
        {
            double ret = 0.0;
            if (eventoInicial == finDeSubidaCalecita && estadoCalecita == funcionando)
            {
                if (colaCalecita != 0) return 0;

                ret = 15 - colaCalecita;
            }
            else ret = 0;
            return ret;
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
