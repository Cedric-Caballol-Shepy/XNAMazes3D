using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
namespace JeuProjet
{
    public enum Etat {OUVERT,FERME,PORTE};
    public enum Conversion { DecToBin4, DecToBin8, BinToDec4, BinToDec8, BinToHex8, HexToBin8 ,DecToHex, INVALIDE}
    public class Mur
    {
        public Etat state { get; private set; }
        public Conversion typeConversion { get; private set; }
        public String code;
        public Mur(){
            this.typeConversion = Conversion.INVALIDE;
            this.state = Etat.FERME;
            this.code = "";
        }
        public void ouvrir()
        {
            this.state = Etat.OUVERT;
            this.code = "";
            this.typeConversion = Conversion.INVALIDE;
        }
        public void fermer()
        {
            this.state = Etat.FERME;
            this.code = "";
            this.typeConversion = Conversion.INVALIDE;
        }
        /// <summary>
        /// transforme un mur ouvert en porte
        /// </summary>
        /// <param name="nbChoixConv">nombre de conversions possibles</param>
        public void transformerEnPorte(int nbChoixConv)
        {
            this.state = Etat.PORTE;
            this.typeConversion = choixConversion(nbChoixConv);
            genereCode();
            
            //test :
            //this.typeConversion = Conversion.BinToHex8;
            //this.code = "10101010"; //en hexa : AA
            
        }
        /// <summary>
        /// Va choisir aléatoirement quel type de conversion le joueur devra faire pour ouvrir la porte (maximum 7)
        /// </summary>
        /// <param name="nbChoix">Un nombre de choix réduit permettra de mettre moins de types (+ toujours les mêmes) et donc rendra le jeu plus facile</param>
        public Conversion choixConversion(int nbChoix)
        {
            int conv = Game1.r.Next(1,nbChoix+1); //de 1 à nbChoix inclu
            switch (conv)
            {
                case 1: return Conversion.DecToHex;
                case 2: return Conversion.DecToBin4;
                case 3: return Conversion.BinToDec4;
                case 4: return Conversion.BinToHex8;
                case 5: return Conversion.HexToBin8;
                case 6: return Conversion.DecToBin8;
                case 7: return Conversion.BinToDec8;
                default: return Conversion.INVALIDE;
            }
        }
        public void genereCode()
        {
            int a, b, c, d, e, f, g, h, i;
            a = Game1.r.Next(2); // 0 ou 1
            b = Game1.r.Next(2);
            c = Game1.r.Next(2);
            d = Game1.r.Next(2);
            e = Game1.r.Next(2);
            f = Game1.r.Next(2);
            g = Game1.r.Next(2);
            h = Game1.r.Next(2);
            switch (this.typeConversion)
            {
                case Conversion.DecToBin4 :
                    i = Game1.r.Next(16);
                    this.code = i.ToString();
                    break;
                case Conversion.BinToDec4 :
                    this.code = a.ToString() + b.ToString() + c.ToString() + d.ToString();
                    break;
                case Conversion.BinToHex8 :
                    this.code = a.ToString() + b.ToString() + c.ToString() + d.ToString()
                        + e.ToString() + f.ToString() + g.ToString() + h.ToString();
                    break;
                case Conversion.HexToBin8 :
                    String hex = "0123456789ABCDEF";
                    this.code = hex[Game1.r.Next(17)].ToString() + hex[Game1.r.Next(17)].ToString();
                    break;
                case Conversion.DecToBin8 :
                    i = Game1.r.Next(256);
                    this.code = i.ToString();
                    break;
                case Conversion.BinToDec8 :
                    this.code = a.ToString() + b.ToString() + c.ToString() + d.ToString()
                        + e.ToString() + f.ToString() + g.ToString() + h.ToString();
                    break;
                case Conversion.DecToHex :
                    i = Game1.r.Next(16);
                    this.code = i.ToString();
                    break;
                default: 
                    this.code = "";
                    break;
            }
        }
        private bool decToBin(String reponse)
        {
            return ((Int32)Convert.ToInt32(reponse, 2) == (Int32)int.Parse(this.code));
        }
        private bool binToDec(String reponse)
        {
            return (((Int32)int.Parse(reponse)).Equals(Convert.ToInt32(this.code, 2)));
        }
        private bool binToHex(String reponse)
        {
            return (Convert.ToInt32(reponse, 16) == Convert.ToInt32(this.code, 2));
        }
        private bool hexToBin(String reponse)
        {
            return (Convert.ToInt32(reponse, 2) == Convert.ToInt32(this.code, 16));
        }
        private bool decToHex(String reponse)
        {
            return ((Convert.ToInt32(reponse, 16) == (Int32)int.Parse(this.code)));
        }
        public bool convertir(String reponse)
        {
            try
            {
                switch (this.typeConversion)
                {
                    case Conversion.DecToBin4 :
                        return decToBin(reponse);
                    case Conversion.DecToBin8 :
                        return decToBin(reponse);
                    case Conversion.BinToDec4 :
                        return binToDec(reponse);
                    case Conversion.BinToDec8 :
                        return binToDec(reponse);
                    case Conversion.BinToHex8:
                        return binToHex(reponse);
                    case Conversion.HexToBin8:
                        return hexToBin(reponse);
                    case Conversion.DecToHex:
                        return decToHex(reponse);
                    default : return false;
                }
            }
            catch (Exception)
            {
                //possible division par 0 lors des conversions (si le joueur écrit n'importe-quoi)  (c'est déjà arrivé lors d'un test)
            }
            return false;
        }
        public String donneTypeConv()
        {
            switch (this.typeConversion)
            {
                case Conversion.DecToBin4:
                    return "decimal->binaire(4)";
                case Conversion.BinToDec4:
                    return "binaire->decimal";
                case Conversion.BinToHex8:
                    return "binaire->hexadecimal";
                case Conversion.HexToBin8:
                    return "hexadecimal->binaire(8)";
                case Conversion.DecToBin8:
                    return "decimal->binaire(8)";
                case Conversion.BinToDec8:
                    return "binaire(8)->decimal";
                case Conversion.DecToHex :
                    return "decimal->hexadecimal";
                default:
                    return "";
            }
        }
    }
}
