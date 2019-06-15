using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP5_SIM
{
    class Niño
    {


        private double estado;
        private double rnd;
        private double tiempoSubida;

        public Niño(double estado, double rnd, double tiempoSubida)
        {
            this.estado = estado;
            this.rnd = rnd;
            this.tiempoSubida = tiempoSubida;
        }

        public void setEstado(double estado)
        {
            this.estado = estado;
        }
        public void setRnd(double rnd)
        {
            this.rnd = rnd;
        }
        public void setTiempoSubida(double tiempoSubida)
        {
            this.tiempoSubida = tiempoSubida;
        }
        public double GetEstado()
        {
            return estado;
        }

        public double GetRnd()
        {
            return rnd;
        }
        public double GetTiempoSubida()
        {
            return tiempoSubida;
        }
    }
}
