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
        bool primerIteracion;
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

            bool primeraVez = true;
            bool primeraVezProximoRompeEnLlanto = true;
            double evento = eventoInicial;
            double reloj = 0.0;
            double rndTiempoLlegada = -1;
            double tiempoLlegada = -1;
            double proxLlegada = 0.19;
            double rndTieneFichas = -1;
            double tieneFichas = -1;
            double rndCantNiños = -1;
            double cantNiños = -1;
            double rndFinDeCompra = -1;
            double tiempoFinDeCompra = -1;
            double proxFinDeCompra = -1;
            double tiempoFinSubidaCal = -1;
            double tiempoRompeEnLlanto = -1;
            double taParaLlorar = -1;
            double proximoRompeEnLlanto = -1;
            double estadoCalecita = 0.0; 
            double colaCalecita = 8;
            double proxVueltaCalecita = -1;
            double estadoBoleteria = this.estadoBoleteriaLibre;
            double colaBoleteria = 0;
            double acuTiempoFuncionamiento = 0;
            double lugaresVacios = 0;
            double cantidadFichasNoCompradas = 0;
            double cantidadFichasCompradas = 0;

            for (int j = 0; j < 7; j++)
            {
                listaAuxiliarNiños.AddFirst(new Niño(3, -1, -1));
            }           

            int cont = 0;
            int nroExp = 1;

            for (int i = 1; i <= cantSimulaciones; i++)
            {
                if (nroExp >= desde && cont <= hasta)
                {
                    if ((cantSimulaciones - desde) < 100)
                    {
                        hasta = cantSimulaciones - desde;
                    }

                    datos[cont, 0] = evento;
                    datos[cont, 1] = reloj;
                    datos[cont, 2] = rndTiempoLlegada;
                    datos[cont, 3] = tiempoLlegada;
                    datos[cont, 4] = proxLlegada;
                    datos[cont, 5] = rndTieneFichas;
                    datos[cont, 6] = tieneFichas;
                    datos[cont, 7] = rndCantNiños;
                    datos[cont, 8] = cantNiños;
                    datos[cont, 9] = rndFinDeCompra;
                    datos[cont, 10] = tiempoFinDeCompra;
                    datos[cont, 11] = proxFinDeCompra;
                    datos[cont, 12] = tiempoFinSubidaCal;
                    datos[cont, 13] = tiempoRompeEnLlanto;
                    datos[cont, 14] = taParaLlorar;
                    datos[cont, 15] = proximoRompeEnLlanto;
                    datos[cont, 16] = estadoCalecita;
                    datos[cont, 17] = colaCalecita;
                    datos[cont, 18] = proxVueltaCalecita;
                    datos[cont, 19] = estadoBoleteria;
                    datos[cont, 20] = colaBoleteria;
                    datos[cont, 21] += acuTiempoFuncionamiento;
                    datos[cont, 22] = lugaresVacios;
                    datos[cont, 23] = cantidadFichasNoCompradas;
                    datos[cont, 24] = cantidadFichasCompradas;
                    GestorNiño(evento, tieneFichas, estadoCalecita, colaCalecita, listaAuxiliarNiños, cantNiños);

                    cont++;
                }

                if (i != cantSimulaciones)
                {
                    nroExp = (i + 1);
                    evento = SetEvento(proxLlegada, proxFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxVueltaCalecita);
                    reloj = CalculoReloj(evento, proxLlegada, proxFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxVueltaCalecita);
                    rndTiempoLlegada = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    tiempoLlegada = (rndTiempoLlegada != -1) ? CalcularTiempoLlegada(1, 5, rndTiempoLlegada) : -1;
                    proxLlegada = (tiempoLlegada != -1) ? reloj + tiempoLlegada : proxLlegada;
                    rndTieneFichas = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    tieneFichas = (rndTieneFichas != -1) ? TieneFichas(rndTieneFichas) : -1;
                    rndCantNiños = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    cantNiños = (rndCantNiños != -1) ? CalcularCantNiños(1, 5, rndCantNiños) : -1; 
                    rndFinDeCompra = (tieneFichas == noTieneFicha) ? GenerarRandom() : -1;
                    tiempoFinDeCompra = (rndFinDeCompra != -1) ? CalcularTiempoFinDeCompra(1, 2, rndFinDeCompra) : -1;
                    proxFinDeCompra = (tiempoFinDeCompra != -1) ? reloj + tiempoFinDeCompra : -1;
                    tiempoFinSubidaCal = (estadoCalecita == detenida && evento != finDeSubidaCalecita) ? CalcularTiempoFinSubidaCalecita(listaAuxiliarNiños, reloj) : -1;
                    tiempoRompeEnLlanto = (evento == finDeSubidaCalecita) ? reloj : -1;
                    taParaLlorar = CalculoTaParaLlorar(evento, tiempoRompeEnLlanto, taParaLlorar, reloj, primeraVez);
                    proximoRompeEnLlanto = (tiempoRompeEnLlanto != -1) ? 4.5 + reloj : -1;
                    estadoCalecita = CalculoEstadoCalecita(evento, colaCalecita, estadoCalecita, reloj, proxVueltaCalecita);
                    colaCalecita = CalculoColaCalecita(evento, colaCalecita, cantNiños, estadoCalecita);
                    proxVueltaCalecita = CalculoProxVueltaCalecita(evento, reloj, proxVueltaCalecita, estadoCalecita);
                    estadoBoleteria = CalculoEstadoBoleteria(evento, tieneFichas, estadoBoleteria, colaBoleteria);
                    colaBoleteria = AgregarFamiliaColaBoleteria(evento, estadoBoleteria, colaBoleteria, tieneFichas);
                    acuTiempoFuncionamiento += CalculoAcuTiempoFuncionamiento(evento);
                    lugaresVacios = CalculoLugaresVacios(estadoCalecita, evento, colaCalecita);
                    cantidadFichasNoCompradas = (evento == llegadaFamilia && tieneFichas == siTieneFicha) ? cantidadFichasNoCompradas += cantNiños : cantidadFichasNoCompradas;
                    cantidadFichasCompradas = (evento == llegadaFamilia && tieneFichas == noTieneFicha) ? cantidadFichasCompradas += cantNiños : cantidadFichasCompradas;

                    GestorNiño(evento, tieneFichas, estadoCalecita, colaCalecita, listaAuxiliarNiños, cantNiños);
                }
            }

            this.matrizDatos = datos;
        }
       
        private double CalculoProxRompeEnLlanto(double evento, double reloj, double estadoCalecita, bool primeraVezRompeEnLlanto, double proxRompeEnLlantoActual)
        {
            double ret = -1;

            if (estadoCalecita == funcionando && primeraVezRompeEnLlanto == true)
            {
                ret = reloj + 4.5;
                primeraVezRompeEnLlanto = false;
            }
            else if (estadoCalecita == funcionando && evento != finDeVueltaCalecita) ret = proxRompeEnLlantoActual;
            else if (estadoCalecita == detenida && primeraVezRompeEnLlanto == false) ret = -1;
            else if (evento == rompeEnLlanto && primeraVezRompeEnLlanto == false) ret = reloj + 4.5;
            

            return ret;
        }

        private double CalculoEstadoCalecita(double evento, double colaCalecita, double estadoActual, double reloj, double proxVueltaCalecita)
        {
            double ret = detenida;

            if ((colaCalecita == 0 && evento != finDeVueltaCalecita && evento != rompeEnLlanto) || evento == finDeSubidaCalecita) ret = funcionando;

            if (estadoActual == funcionando && (evento != finDeVueltaCalecita && evento != rompeEnLlanto)) ret = funcionando;

            if (proxVueltaCalecita != -1 && reloj >= proxVueltaCalecita) ret = detenida;

            return ret;
        }

        private double CalculoAcuTiempoFuncionamiento(double evento)
        {
            double retorno = 0.0;
            if (evento == finDeVueltaCalecita) retorno = 4.0;
            else if (evento == rompeEnLlanto) retorno = 0.5;

            return retorno;
        }

        private double CalculoReloj(double evento, double proximaLlegada, double proxFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxVueltaCalecita)
        {
            double menor = proximaLlegada;
            if (menor > proxFinDeCompra && proxFinDeCompra != -1) menor = proxFinDeCompra;
            else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            else if (menor > proxVueltaCalecita && proxVueltaCalecita != -1) menor = proxVueltaCalecita;

            return menor;
        }
        private double SetEvento(double proximaLlegada, double proxFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxVueltaCalecita)
        {
            double menor = proximaLlegada;
            if (menor > proxFinDeCompra && proxFinDeCompra != -1) menor = proxFinDeCompra;
            else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            else if (menor > proxVueltaCalecita && proxVueltaCalecita != -1) menor = proxVueltaCalecita;

            if (menor == proximaLlegada) return llegadaFamilia;
            else if (menor == proxFinDeCompra) return finDeCompra;
            else if (menor == tiempoFinSubidaCal) return finDeSubidaCalecita;
            else if (menor == proximoRompeEnLlanto) return rompeEnLlanto;
            else return finDeVueltaCalecita;
        }
        private double CalcularTiempoFinSubidaCalecita(LinkedList<Niño> listaNiños, double reloj)
        {
            double acu = 0;
            if (listaNiños.Count == 0) return acu;
            else
            {                
                foreach (Niño n in listaNiños)
                {
                    acu += (n.GetEstado() == subiendo && n.GetTiempoSubida() != -1) ? n.GetTiempoSubida() : 0;
                }
            }
            return reloj + acu;
        }
        private void GestorNiño(double evento, double tieneFicha, double estadoCalecita, double colaCalecita, LinkedList<Niño> lista, double cantidadNiñosLlegan)
        {
            double rnd = GenerarRandom();
            if (evento == eventoInicial)
            {
                foreach (Niño n in lista)
                {
                    n.setEstado(0);
                    n.setRnd(rnd);
                    n.setTiempoSubida(CalcularTiempoSubida(8, 18, rnd));

                }
            }
            if (cantidadNiñosLlegan < 0) return;           
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
            //if (evento == finDeVueltaCalecita)
            //{
            //    foreach (Niño n in lista)
            //    {
            //        if (n.GetEstado() == enCalecita) lista.Remove(n);
            //    }
            //}
            if (evento == llegadaFamilia && tieneFicha == siTieneFicha && estadoCalecita == funcionando)
            {
                lista.AddFirst(new Niño(enColaCalecita, -1, -1));
            }
        }

        private double CalcularTiempoSubida(double a, double b, double rnd)
        {
            return (double) Math.Truncate(a + (rnd * (b - a))) / 100;
        }

        private double CalculoTaParaLlorar(double evento, double tiempoRompeEnLlanto, double taParaLlorarActual, double reloj, bool primeraVez)
        {
            double ret = -1;
            if (tiempoRompeEnLlanto != -1 && primeraVez == true)
            {
                primeraVez = false;
                return 4.5;               
            }

            if (evento != finDeVueltaCalecita)
            {
                ret = 4.5;
            }
            else ret = taParaLlorarActual - 4.0;
            if (evento == rompeEnLlanto) ret = 4.5;
 
            
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

        private double AgregarFamiliaColaBoleteria(double eventoActual, double estadoBoleteriaActual, double colaBoleteria, double tieneFicha)
        {
            if (estadoBoleteriaActual == estadoBoleteriaOcupado && eventoActual == llegadaFamilia && tieneFicha == noTieneFicha)
            {
                return colaBoleteria + 1;
            }
            else if (eventoActual == finDeCompra) return colaBoleteria - 1;

            return colaBoleteria;
        }

        private double CalculoEstadoBoleteria(double evento, double tieneFicha, double estadoBoleteriaActual, double colaBoleteria)
        {
            if (evento == this.llegadaFamilia && tieneFicha == siTieneFicha && estadoBoleteriaActual == estadoBoleteriaLibre)
            {
                return estadoBoleteriaOcupado;
            }
            if (estadoBoleteriaActual == estadoBoleteriaOcupado && evento == finDeCompra && colaBoleteria == 0) return estadoBoleteriaLibre;

            return estadoBoleteriaOcupado;
        }

        private double CalculoProxVueltaCalecita(double evento, double reloj, double proxVueltaCalecitaActual, double estadoCalecita)
        {
            double auxiliar = -1;

            if (evento == this.finDeSubidaCalecita)
            {
                auxiliar = reloj + 4.0;
            }
            else if (evento == rompeEnLlanto)
            {
                auxiliar = proxVueltaCalecitaActual + 0.3;
            }
            else if (evento == finDeVueltaCalecita)
            {
                auxiliar = -1;
            }
            else if (estadoCalecita == funcionando && proxVueltaCalecitaActual != -1)
            {
                auxiliar = proxVueltaCalecitaActual;
            }

            return auxiliar;
        }

        private double CalculoColaCalecita(double evento, double colaCalecitaActual, double cantNiños, double estadoCalecita)
        {
            double retorno = -1;
            if (estadoCalecita == this.detenida && cantNiños != -1)
            {
                retorno = colaCalecitaActual + cantNiños;
            }

            if (evento == finDeSubidaCalecita && (colaCalecitaActual + cantNiños) >= 15)
            {
                retorno = colaCalecitaActual - 15;
            }
            else retorno = 0;

            return retorno;
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
            double retorno = (double)Math.Truncate((random.NextDouble() * 10000)) / 10000;
            return retorno;
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
