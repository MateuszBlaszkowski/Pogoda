using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiApp4
{
    public class Stations
    {
        public string id_stacji {  get; set; }
        public string stacja {  get; set; }
        public string data_pomiaru {  get; set; }
        public string godzina_pomiaru {  get; set; }
        public string temperatura {  get; set; }
        public string predkosc_wiatru {  get; set; }
        public string kierunek_wiatru {  get; set; }
        public string wilgotnosc_wzgledna {  get; set; }
        public string suma_opadu {  get; set; }
        public string cisnienie {  get; set; }
    }
}
