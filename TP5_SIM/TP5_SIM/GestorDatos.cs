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
        private double[,] lugaresCalecita = new double[3, 15];
        private Random random = new Random();
        bool primerIteracion;
        LinkedList<double> listaCantDeNiños = new LinkedList<double>();       

        public GestorDatos()
        {
            matrizDatos = new double[100, 26];
        }

        public void CargarDatos(int cantSimulaciones, int desde)
        {
            uint hasta = 100;
            double[,] datos = new double[101, 26];
            LinkedList<Niño> listaAuxiliarNiños = new LinkedList<Niño>();
            GestorNiños gestorNiños = new GestorNiños(listaAuxiliarNiños);          
            bool primeraVez = true;
            double evento = -1;
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
            double altaParaLlorar = -1;
            double proximoRompeEnLlanto = -1;
            double estadoCalecita = 0.0;
            double colaCalecita = 8;
            double proxFinDeVueltaCalecita = -1;
            double estadoBoleteria = 0;
            double colaBoleteria = -1;
            double acuTiempoFuncionamiento = 0;
            double lugaresVacios = 0;
            double cantidadFichasNoCompradas = 0;
            double cantidadFichasCompradas = 0;
            int cont = 0;
            int nroExp = 1;


            for (int j = 0; j < 8; j++)
            {
                listaAuxiliarNiños.AddFirst(new Niño(3, -1, -1));
            }           

            

            for (int i = 1; i <= cantSimulaciones; i++)
            {
                if (nroExp >= desde && cont <= hasta)
                {
                    if ((cantSimulaciones - desde) < 100)
                    {
                        hasta = (uint)(cantSimulaciones - desde);
                    }

                    datos[cont, 0] = nroExp;
                    datos[cont, 1] = evento;
                    datos[cont, 2] = reloj;
                    datos[cont, 3] = rndTiempoLlegada;
                    datos[cont, 4] = tiempoLlegada;
                    datos[cont, 5] = proxLlegada;
                    datos[cont, 6] = rndTieneFichas;
                    datos[cont, 7] = tieneFichas;
                    datos[cont, 8] = rndCantNiños;
                    datos[cont, 9] = cantNiños;
                    datos[cont, 10] = rndFinDeCompra;
                    datos[cont, 11] = tiempoFinDeCompra;
                    datos[cont, 12] = proxFinDeCompra;
                    datos[cont, 13] = tiempoFinSubidaCal;
                    datos[cont, 14] = tiempoRompeEnLlanto;
                    datos[cont, 15] = altaParaLlorar;
                    datos[cont, 16] = proximoRompeEnLlanto;
                    datos[cont, 17] = estadoCalecita;
                    datos[cont, 18] = colaCalecita;
                    datos[cont, 19] = proxFinDeVueltaCalecita;
                    datos[cont, 20] = estadoBoleteria;
                    datos[cont, 21] = colaBoleteria;
                    datos[cont, 22] += acuTiempoFuncionamiento;
                    datos[cont, 23] = lugaresVacios;
                    datos[cont, 24] = cantidadFichasNoCompradas;
                    datos[cont, 25] = cantidadFichasCompradas;
                    GestorNiño(evento, tieneFichas, estadoCalecita, colaCalecita, listaAuxiliarNiños, cantNiños, lugaresCalecita);

                    cont++;
                }

                if (i != cantSimulaciones)
                {
                    nroExp = (i + 1);
                    evento = SetEvento(proxLlegada, proxFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxFinDeVueltaCalecita);
                    reloj = CalculoReloj(evento, proxLlegada, proxFinDeCompra, tiempoFinSubidaCal, proximoRompeEnLlanto, proxFinDeVueltaCalecita);
                    rndTiempoLlegada = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    tiempoLlegada = (rndTiempoLlegada != -1) ? CalcularTiempoLlegada(1, 5, rndTiempoLlegada) : -1;
                    proxLlegada = (tiempoLlegada != -1) ? reloj + tiempoLlegada : proxLlegada;
                    rndTieneFichas = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    tieneFichas = (rndTieneFichas != -1) ? TieneFichas(rndTieneFichas) : -1;
                    rndCantNiños = (evento == llegadaFamilia) ? GenerarRandom() : -1;
                    cantNiños = (rndCantNiños != -1) ? CalcularCantNiños(1, 5, rndCantNiños) : -1;
                    rndFinDeCompra = DispararRndFinDeCompra(evento, tieneFichas, estadoBoleteria, colaBoleteria);
                    tiempoFinDeCompra = (rndFinDeCompra != -1) ? CalcularTiempoFinDeCompra(1, 2, rndFinDeCompra) : -1;
                    //proxFinDeCompra = (tiempoFinDeCompra != -1) ? reloj + tiempoFinDeCompra : -1;
                    proxFinDeCompra = CalculoProxFinDeCompra(evento, tieneFichas, reloj, tiempoFinDeCompra, colaBoleteria, proxFinDeCompra);
                    //tiempoFinSubidaCal = (estadoCalecita == detenida && evento != finDeSubidaCalecita) ? CalcularTiempoFinSubidaCalecita(listaAuxiliarNiños, reloj) : -1;
                    tiempoFinSubidaCal = CalculoTiempoFinDeSubidaCal(evento, tiempoFinSubidaCal, listaAuxiliarNiños, reloj, estadoCalecita, tieneFichas);
                    tiempoRompeEnLlanto = (evento == finDeSubidaCalecita) ? reloj : -1;
                    altaParaLlorar = CalculoTaParaLlorar(evento, tiempoRompeEnLlanto, altaParaLlorar, reloj, primeraVez);
                    proximoRompeEnLlanto = CalculoProxRompeEnLlanto(evento, reloj, proximoRompeEnLlanto, altaParaLlorar);
                    estadoCalecita = CalculoEstadoCalecita(evento, colaCalecita, estadoCalecita, reloj, proxFinDeVueltaCalecita);
                    colaCalecita = CalculoColaCalecita(evento, colaCalecita, cantNiños, estadoCalecita, tieneFichas);
                    proxFinDeVueltaCalecita = CalculoProxVueltaCalecita(evento, reloj, proxFinDeVueltaCalecita, estadoCalecita);
                    estadoBoleteria = CalculoEstadoBoleteria(evento, tieneFichas, estadoBoleteria, colaBoleteria);
                    colaBoleteria = AgregarFamiliaColaBoleteria(evento, estadoBoleteria, colaBoleteria, tieneFichas);
                    acuTiempoFuncionamiento += CalculoAcuTiempoFuncionamiento(evento);
                    lugaresVacios = CalculoLugaresVacios(estadoCalecita, evento, colaCalecita);
                    cantidadFichasNoCompradas = (evento == llegadaFamilia && tieneFichas == siTieneFicha) ? cantidadFichasNoCompradas += cantNiños : cantidadFichasNoCompradas;
                    cantidadFichasCompradas = (evento == llegadaFamilia && tieneFichas == noTieneFicha) ? cantidadFichasCompradas += cantNiños : cantidadFichasCompradas;

                    GestorNiño(evento, tieneFichas, estadoCalecita, colaCalecita, listaAuxiliarNiños, cantNiños, lugaresCalecita);
                }
            }

            this.matrizDatos = datos;
        }
       
        private double CalculoTiempoFinDeSubidaCal(double evento, double tiempoFinDeSibidaCalActual, LinkedList<Niño> lista, double reloj, double estadoCalecita, double tieneFicha)
        {
            double retorno = -1;

            if ((evento != finDeSubidaCalecita && estadoCalecita == detenida) || evento == finDeVueltaCalecita)
            {
                retorno = CalcularTiempoFinSubidaCalecita(lista, reloj);
            }

            if (evento == rompeEnLlanto) retorno = -1;

            if (evento == llegadaFamilia && tieneFicha == noTieneFicha) retorno = tiempoFinDeSibidaCalActual;

            if (evento == finDeCompra)
            {
                lista.AddFirst(new Niño(2, -1, -1));
                //retorno = CalcularTiempoFinSubidaCalecita(lista, reloj);
            }
            return retorno;
        }

        private double CalculoProxFinDeCompra(double evento, double tieneFicha, double reloj, double tiempoFinDeCompra, double colaBoleteria, double proxFinDeCompraActual)
        {
            double retorno = -1;

            if (tiempoFinDeCompra != -1) retorno = reloj + tiempoFinDeCompra; else retorno = -1;

            if (evento != llegadaFamilia && evento != finDeCompra && proxFinDeCompraActual != -1)
            {
                retorno = proxFinDeCompraActual;
            }
            else return retorno;

            //if (evento == finDeCompra && colaBoleteria == 0)
            //{
            //    if (proxFinDeCompraActual == -1) retorno = -1;
            //    else
            //    {
            //        retorno = proxFinDeCompraActual;
            //    }
            //}

            //if (evento == finDeCompra && colaBoleteria > 0)
            //{
            //    retorno = tiempoFinDeCompra + reloj;
            //}
            return retorno;
        }

        private double CalculoProxRompeEnLlanto(double evento, double reloj, double proximoRompeEnLlantoActual, double taParaLlorar)
        {
            double ret = proximoRompeEnLlantoActual;
            if (evento == finDeSubidaCalecita && proximoRompeEnLlantoActual == -1)
            {
                if (taParaLlorar == 4.5) ret = reloj + 4.5;
                else ret = reloj + 0.5;
            }

            if (proximoRompeEnLlantoActual != -1 && evento != finDeVueltaCalecita) ret = proximoRompeEnLlantoActual;

            if (evento == finDeVueltaCalecita) ret = -1;

            if (evento == rompeEnLlanto) ret = reloj + 4.5;

            return ret;
        }

        private double DispararRndFinDeCompra(double evento, double tieneFicha, double estadoBoleteria, double colaBoleteria)
        {
            double rnd = -1;
            if (evento == llegadaFamilia && tieneFicha == noTieneFicha)
            {
                rnd = GenerarRandom();
            }

            if (evento == finDeCompra && colaBoleteria == -1)
            {
                rnd = GenerarRandom();
            }

            return rnd;
        }

        private double CalculoEstadoCalecita(double evento, double colaCalecita, double estadoActual, double reloj, double proxVueltaCalecita)
        {
            double ret = detenida;


            if ((estadoActual == detenida && colaCalecita == 0 && evento != finDeVueltaCalecita && evento != rompeEnLlanto) || evento == finDeSubidaCalecita) ret = funcionando;

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

        private double CalculoReloj(double evento, double proximaLlegada, double proxFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxFinDeVueltaCalecita)
        {
            double menor = proximaLlegada;
            double[] aux = new double[5];

            aux[0] = proximaLlegada;
            aux[1] = proxFinDeCompra;
            aux[2] = tiempoFinSubidaCal;
            aux[3] = proximoRompeEnLlanto;
            aux[4] = proxFinDeVueltaCalecita;

            for (int i = 0; i < aux.GetLength(0); i++)
            {
                if (menor > aux[i] && aux[i] != -1)
                {
                    menor = aux[i];
                }
            }
            
            //if (menor > proxFinDeCompra && proxFinDeCompra != -1) menor = proxFinDeCompra;
            //else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            //else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            //else if (menor > proxFinDeVueltaCalecita && proxFinDeVueltaCalecita != -1)
            //{
            //    menor = proxFinDeVueltaCalecita;
            //}
            return menor;
        }
        private double SetEvento(double proximaLlegada, double proxFinDeCompra, double tiempoFinSubidaCal, double proximoRompeEnLlanto, double proxFinDeVueltaCalecita)
        {
            double menor = proximaLlegada;
            //if (menor > proxFinDeCompra && proxFinDeCompra != -1) menor = proxFinDeCompra;
            //else if (menor > tiempoFinSubidaCal && tiempoFinSubidaCal != -1) menor = tiempoFinSubidaCal;
            //else if (menor > proximoRompeEnLlanto && proximoRompeEnLlanto != -1) menor = proximoRompeEnLlanto;
            //else if (menor > proxVueltaCalecita && proxVueltaCalecita != -1) menor = proxVueltaCalecita;
            double[] aux = new double[5];

            aux[0] = proximaLlegada;
            aux[1] = proxFinDeCompra;
            aux[2] = tiempoFinSubidaCal;
            aux[3] = proximoRompeEnLlanto;
            aux[4] = proxFinDeVueltaCalecita;

            for (int i = 0; i < aux.GetLength(0); i++)
            {
                if (menor > aux[i] && aux[i] != -1)
                {
                    menor = aux[i];
                }
            }

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
        //private double CalcularTiempoFinSubidaCalecita(LinkedList<Niño> listaNiños, double reloj, double cantNiños)
        //{
        //    double acu = 0;
        //    if (listaNiños.Count == 0) return acu;
        //    else
        //    {
        //        for (int i = 0; i < cantNiños; i++)
        //        {
        //            acu += (n.GetEstado() == subiendo && n.GetTiempoSubida() != -1) ? n.GetTiempoSubida() : 0;
        //        }
        //    }
        //    return reloj + acu;
        //}
        private void GestorNiño(double evento, double tieneFicha, double estadoCalecita, double colaCalecita, LinkedList<Niño> lista, double cantidadNiñosLlegan, double[,] lugaresCalecita)
        {
            int i = 0;
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
            //if (evento == llegadaFamilia && tieneFicha == noTieneFicha)
            //{
            //    lista.AddFirst(new Niño(enColaCompra, -1, -1));
            //}
            if (evento == finDeSubidaCalecita)
            {
                foreach (Niño n in lista)
                {                 
                    if (colaCalecita <= 15)
                    {
                        if (n.GetEstado() == subiendo)
                        {
                            n.setEstado(enCalecita);
                            n.setRnd(-1);
                            n.setTiempoSubida(-1);

                            lugaresCalecita[i, 0] = n.GetEstado();
                            lugaresCalecita[i, 1] = n.GetRnd();
                            lugaresCalecita[i, 2] = n.GetTiempoSubida();
                        }                      
                    }
                    i++;
                }
            }
            if (evento == finDeVueltaCalecita)
            {
                for (int j = 0; j < lugaresCalecita.GetLength(0); j++)
                {
                    for (int k = 0; k < lugaresCalecita.GetLength(1); k++)
                    {
                        if (lugaresCalecita[j, k] == 1) lugaresCalecita[j, k] = -1;
                    }
                }
                foreach (Niño n in lista)
                {
                    if (n.GetEstado() == enCalecita)
                    {
                        n.setEstado(-1);
                    }
                }
            }
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

            if (taParaLlorarActual == -1) ret = -1;

            if ((evento == finDeSubidaCalecita || taParaLlorarActual != -1) && evento != finDeVueltaCalecita) ret = 4.5;

            if (evento == finDeVueltaCalecita) ret = taParaLlorarActual - 4.0;

            if (taParaLlorarActual == 0.5) ret = 0.5;

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
            if (colaBoleteria == -1 && eventoActual == llegadaFamilia && tieneFicha == noTieneFicha) return 0;

            if (colaBoleteria > -1 && eventoActual == llegadaFamilia && tieneFicha == noTieneFicha)
            {
                return colaBoleteria + 1;
            }
            if (eventoActual == finDeCompra) return colaBoleteria - 1;
            if (eventoActual == finDeCompra && colaBoleteria == 0) return -1;


            return colaBoleteria;
        }

        private double CalculoEstadoBoleteria(double evento, double tieneFicha, double estadoBoleteriaActual, double colaBoleteria)
        {

            if (evento == llegadaFamilia && tieneFicha == noTieneFicha && colaBoleteria == -1) return estadoBoleteriaOcupado;

            if (evento == llegadaFamilia && tieneFicha == noTieneFicha && colaBoleteria > -1) return estadoBoleteriaOcupado;

            if (estadoBoleteriaActual == estadoBoleteriaOcupado && evento == finDeCompra && colaBoleteria == 0) return estadoBoleteriaLibre;

            return estadoBoleteriaActual;            
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
            else auxiliar = proxVueltaCalecitaActual;

            return auxiliar;
        }

        private double CalculoColaCalecita(double evento, double colaCalecitaActual, double cantNiños, double estadoCalecita, double tieneFicha)
        {
            double retorno = colaCalecitaActual;

            //if ((evento == llegadaFamilia && tieneFicha == siTieneFicha)) || evento == finDeCompra)
            if ((evento == llegadaFamilia && tieneFicha == siTieneFicha))
            {
                retorno = colaCalecitaActual + cantNiños;
            }
            else
            {
                if (tieneFicha == noTieneFicha)
                {
                    listaCantDeNiños.AddFirst(cantNiños);
                }
            }

            if (evento == finDeCompra)
            {
                retorno = colaCalecitaActual + listaCantDeNiños.Last();

                listaCantDeNiños.RemoveLast();
            }

            if (evento == finDeSubidaCalecita)
            {
                if ((colaCalecitaActual + cantNiños) >= 15) retorno = colaCalecitaActual - 15;
                else retorno = 0;
            }

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

        public double[,] GetLugaresCalecita()
        {
            return lugaresCalecita;
        }
    }
}
