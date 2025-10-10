using System;
using System.Collections.Generic;
using System.IO;



class game
{
    static public player j1 = new player { id = 1, score = 0 };
    static public player j2 = new player { id = 2, score = 0 };


    public struct player
    {
        public int id;
        public string pseudo;
        public int score;
        public List<string> challenges;
        public Dictionary<string, int> totalMineur;
        public bool bonus;
        public bool bonusCompleted;
    }

    public struct Round
    {
        public int idPlayer;
        public int[] Des;
        public string challengeChoise;
        public int Score;
    

        public Round(int id, int[] des, string challenge, int score)
        {
            idPlayer = id;
            Des = des;
            challengeChoise = challenge;
            Score = score;
        }

    }


    static List<Round> historique = new List<Round>();


    static void Main()
    {
        
        
        initGame();

        // jouerTour(ref j1);
        
        for(int i = 0; i < 13; i++)
        {
            string texte = "========== Tour n°" + (i+1) + " ==========";

            int largeurConsole = Console.WindowWidth;
            int positionDebut = (largeurConsole - texte.Length) / 2;

            positionDebut = Math.Max(0, positionDebut);

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(new string(' ', positionDebut) + texte);
            Console.ResetColor();
            Console.WriteLine();


            jouerTour(ref j1);
            jouerTour(ref j2);
        }

        if(j1.score < j2.score)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j2.pseudo);
            Console.ResetColor();
            Console.Write(" gagne avec le score de ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j2.score);
            Console.ResetColor();
            Console.Write(" points contre les ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j1.score);
            Console.ResetColor();
            Console.Write(" points de ");
            Console.Write(j1.pseudo);
            Console.ResetColor();
        }

        else
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j1.pseudo);
            Console.ResetColor();
            Console.Write(" gagne avec le score de ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j1.score);
            Console.ResetColor();
            Console.Write(" points contre les ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j2.score);
            Console.ResetColor();
            Console.Write(" points de ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(j2.pseudo);
            Console.ResetColor();
        }

        endGame(j1, j2);

    }

    static void jouerTour(ref player player)
    {
        int[] des = new int[5];   
        
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(player.pseudo);
        Console.ResetColor();
        Console.Write(" appuyez sur entrée pour lancer les dés");
        Console.ReadLine();
        Console.WriteLine("\n");
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write($"========== Le joueur : {player.pseudo}  ==========");
        Console.ResetColor();
        
        Console.WriteLine("\nVos challenges restant :");

        for(int i = 0; i<player.challenges.Count; i++)
        {

            if (player.challenges[i].Contains("Nombre"))
            {
                Console.ForegroundColor = ConsoleColor.Blue; // Vert pour "Nombre"
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue; // Rouge pour les autres
            }

            Console.WriteLine((i+1) + ") " + player.challenges[i]);
        }
        Console.ResetColor();
        Console.WriteLine();

        des = lancerDes(5, des); //lancement des 5 des de depart

        afficheDes(des);
                        
        int rerol = 0;
        while(rerol != 2)
        {
            bool estJuste = false;
            string rep = "";

            int firstTime =0;

            while(!estJuste)
            {
                if(firstTime != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Saisissez 'o' ou 'n'");
                    Console.ResetColor();
                }
                

                Console.Write("Voulez-vous relancer certains dés ? (");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("o");
                Console.ResetColor();
                Console.Write("/");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("n");
                Console.ResetColor();
                Console.Write(") ");
                

                rep = Console.ReadLine().ToLower();
                

                if(rep == "o" || rep =="n")
                {
                    estJuste = true;
                }
                firstTime++;
            }

            



            if(rep == "o")
            {
                bool inRange = false;
                bool arret = false;

                
                Console.Write("\nIndiquez les dés à relancer (avec des espaces) : ");
                string[] indiceDesARelancer = Console.ReadLine().Split();


                while(!arret)
                {
                    for(int i = 0; i < indiceDesARelancer.Length; i++)
                    {
                        int.TryParse(indiceDesARelancer[i], out int j);
                        if(j >= 1 && j <= 5)
                        {
                            inRange = true;
                        }
                    }    

                    if(!inRange)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Veuillez entrer des nombres entre 1 et 5 !!");
                        Console.ResetColor();

                        Console.Write("\nIndiquez les dés à relancer (avec des espaces) : ");
                        indiceDesARelancer = Console.ReadLine().Split();
                    }

                    else
                    {
                        arret = true;
                    }
                }

                

                foreach(string index in indiceDesARelancer)
                {
                    if(int.TryParse(index, out int i) && i >= 1 && i <=5)
                    {
                        des[i-1] = relanceDe(); 
                    }
                }

                afficheDes(des);

                rerol ++;
            }


            else
            {
                break;
            }
        }

        Console.WriteLine("\nVos challenges disponible :");

        for(int i = 0; i<player.challenges.Count; i++)
        {

            if (player.challenges[i].Contains("Nombre"))
            {
                Console.ForegroundColor = ConsoleColor.Blue; // Vert pour "Nombre"
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue; // Rouge pour les autres
            }

            Console.WriteLine((i+1) + ") " + player.challenges[i]);
        }

        Console.ResetColor();
        Console.WriteLine();

        Console.Write("Choisissez le challenge à compléter ");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write(" (numéro du challenge) ");
        Console.ResetColor();
        Console.Write(": ");

        int indexChallegechoisie;
        bool estValide = int.TryParse(Console.ReadLine(), out indexChallegechoisie);
        indexChallegechoisie--;

        while(!estValide || indexChallegechoisie < 0 || indexChallegechoisie > player.challenges.Count -1 )
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Saisissez un nombre entre 1 et " + player.challenges.Count + " !");
            Console.ResetColor();

            Console.Write("Choisissez le challenge à compléter ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(" (numéro du challenge) ");
            Console.ResetColor();
            Console.Write(": ");

            estValide = int.TryParse(Console.ReadLine(), out indexChallegechoisie);
            indexChallegechoisie--;
        }

        string challengeChoise = player.challenges[indexChallegechoisie];
        player.challenges.RemoveAt(indexChallegechoisie);

        if(challengeChoise.Contains("Nombre"))
        {
            player.totalMineur[challengeChoise] = addPoint(challengeChoise, des, false);
        }

        int point = addPoint(challengeChoise, des, true);
        player.score += point;
        Console.Write("vous avez désormais ");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write(player.score);
        Console.ResetColor();
        Console.Write(" points !\n\n");

        historique.Add(new Round(player.id, des, convertChallengeJson(challengeChoise), point));

        bool stop = false;
        int x = 1;

        while(!stop && x <= 6)
        {
            if(player.totalMineur["Nombre de " + x] == -1)
            {
                stop = true;
            }

            x++;
        }

        if(!stop)
        {
            player.bonusCompleted = true;
            int total = 0;
            for(int y = 1; y <= 6; y++)
            {
                total += player.totalMineur["Nombre de " + y];
            }

            if(total >= 63 && !player.bonusCompleted)  
            {
                player.score += 35;
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(player.pseudo);
                Console.ResetColor();
                Console.Write(" gagne ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("35");
                Console.ResetColor();
                Console.Write(" points supplémentaire car son ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("total");
                Console.ResetColor();
                Console.Write(" des challenges mineurs dépasse 63 points !\n\n");

                player.bonus = true;
            }

            else if(!player.bonusCompleted)
            {
                Console.Write("Le ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write("total");
                Console.ResetColor();
                Console.Write(" des challenges mineurs de ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(player.pseudo);
                Console.ResetColor();
                Console.Write(" ne dépasse pas 63 points !\n\n");
            }
        }
    }

    static int addPoint(string challenge, int[] des, bool ON)
    {
        int total = 0;

        if(challenge.Contains("Nombre"))
            {
                int val = int.Parse(challenge.Split(' ')[2]);
                total = calculNombre(val, des);
            }

        switch(challenge)
        {
            case "Brelan":
                total = calculBrelan(des);
                break;
            
            case "Carré":
                total = calculCarre(des);
                break;
            
            case "Full":
                total = calculFull(des);
                break;

            case "Petite suite":
                total = calculPetiteSuite(des);
                break;

            case "Grande suite":
                total = calculGrandeSuite(des);
                break;

            case "Yam's":
                total = calculYams(des);
                break;

            case "Chance":
                total = caclulChance(des);
                break;
        }

        if(ON)
        {
            Console.Write("Ce challenge vous octroie ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(total);
            Console.ResetColor();
            Console.Write(" points, ");
        }

        return total;
        
    }

    static string convertChallengeJson(string challenge)
    {
        string nbr;
        string convertedChallenge = "error";

        if(challenge.Contains("Nombre"))
        {
            nbr = challenge.Split(' ')[2];
            convertedChallenge = ("nombre" + nbr);
        }

        switch(challenge)
        {
            case "Brelan":
                convertedChallenge = "brelan";
                break;

            case "Carré":
                convertedChallenge = "carre";
                break;

            case "Full":
                convertedChallenge = "full";
                break;

            case "Petite suite":
                convertedChallenge = "petite";
                break;

            case "Grande suite":
                convertedChallenge = "grande";
                break;

            case "Yam's":
                convertedChallenge = "yams";
                break;

            case "Chance":
                convertedChallenge = "chance";
                break;
        }

        return convertedChallenge;
    }

    static int calculPetiteSuite(int[] des)
    {
        int score = 0;

        trierDes(des);

        int consecutive = 1; 

        for (int i = 1; i < des.Length; i++)
        {
            if (des[i] == des[i - 1] + 1)
            {
                consecutive++;
                if (consecutive >= 4)
                {
                    score = 30; 
                }
            }
            else if (des[i] != des[i - 1]) 
            {
                consecutive = 1;
            }
        }

        return score;
    }

    static int calculGrandeSuite(int[] des)
    {
        int score = 0;

        trierDes(des);

        int consecutive = 1; 

        for (int i = 1; i < des.Length; i++)
        {
            if (des[i] == des[i - 1] + 1)
            {
                consecutive++;
                if (consecutive >= 5)
                {
                    score = 40; 
                }
            }
            else if (des[i] != des[i - 1]) 
            {
                consecutive = 1;
            }
        }

        return score;
    }

    static void trierDes(int[] des)
    {
        for (int i = 1; i < des.Length; i++)
        {
            int nbr = des[i];
            int j = i - 1;

            while (j >= 0 && des[j] > nbr)
            {
                des[j + 1] = des[j];
                j--;
            }

            des[j + 1] = nbr;
        }
    }


    static int calculFull(int[] des)
    {
        bool count2 = false;
        bool count3 = false;
        int score = 0;

        for(int val = 1; val <= 6; val++)
        {
            int count = 0;

            foreach(int de in des)
            {
                if(de == val)
                {
                    count++;
                }
            }

            if(count == 2)
            {
                count2 = true;
            }

            if(count == 3)
            {
                count3 = true;
            }
        }

        if(count2 && count3)
        {
            score = 25;
        }

        return score;


    }

    static int calculYams(int[] des)
    {
        int nbr = des[0];
        bool yams = true;
        int score = 0;

        for(int i = 0;i < des.Length; i++)
        {
            if(des[i] != nbr)
            {
                yams = false;
            }
        }

        if(yams)
        {
            score = 50;
        }

        return score;
    }

    static int calculCarre(int[] des)
    {
        int score = 0;

        for(int i = 1; i <= 6; i++)
        {
            int count = 0;

            for(int y = 0; y < des.Length; y++)
            {
                if(des[y] == i)
                {
                    count ++;
                }
            }

            if(count == 4)
            {
                score =  i * 4;
                break;
            }
        }
        return score;
    }

    static int calculBrelan(int[] des)
    {
        int score = 0;

        for(int i = 1; i <= 6; i++)
        {
            int count = 0;

            for(int y = 0; y < des.Length; y++)
            {
                if(des[y] == i)
                {
                    count ++;
                }
            }

            if(count == 3)
            {
                score =  i * 3;
                break;
            }
        }
        return score;
    }

    static int caclulChance(int[] des)
    {
        int total = 0;

        for(int i = 0; i < des.Length; i++)
        {
            total += des[i];
        }

        return total;
    }

    static int calculNombre(int nbr, int[] des)
    {
        int total = 0;

        for(int i = 0; i < des.Length; i++)
        {
            if(des[i] == nbr)
            {
                total += nbr;
            }
        }

        return total;
    }

    static void afficheDes(int[] tab)
        {
            Console.WriteLine();
            for(int i = 0; i < 5; i++)
            {

                Console.Write("Dé n°" + (i+1) + " = ");     
                Console.ForegroundColor = ConsoleColor.Red;     
                Console.WriteLine(tab[i]);
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.ResetColor();
        }

    static int[] lancerDes(int nbr, int[] tab)
    {
        for(int i = 0; i < nbr; i++)
        {
            tab[i] = relanceDe();
        }

        return tab;
    }

    static int relanceDe()
    {
        Random rnd = new Random();

        return rnd.Next(1,7);
    }



    static void initGame()
    {

        Console.Write("Pseudo du joueur 1 : ");
        j1.pseudo = Console.ReadLine();
        j1.challenges = new List<string>
        {
            "Nombre de 1", "Nombre de 2", "Nombre de 3", "Nombre de 4", "Nombre de 5", "Nombre de 6", "Brelan", "Carré", "Full", "Petite suite", "Grande suite", "Yam's", "Chance"
        };
        j1.totalMineur =  new Dictionary<string, int>
        {
            {"Nombre de 1", -1},
            {"Nombre de 2", -1},
            {"Nombre de 3", -1},
            {"Nombre de 4", -1},
            {"Nombre de 5", -1},
            {"Nombre de 6", -1}
        };
        j1.bonus = false;
        j1.bonusCompleted = false;


        Console.Write("\nPseudo du joueur 2 : ");
        j2.pseudo = Console.ReadLine();
        j2.challenges = new List<string>
        {
            "Nombre de 1", "Nombre de 2", "Nombre de 3", "Nombre de 4", "Nombre de 5", "Nombre de 6", "Brelan", "Carré", "Full", "Petite suite", "Grande suite", "Yam's", "Chance"
        };
        j2.totalMineur =  new Dictionary<string, int>
        {
            {"Nombre de 1", -1},
            {"Nombre de 2", -1},
            {"Nombre de 3", -1},
            {"Nombre de 4", -1},
            {"Nombre de 5", -1},
            {"Nombre de 6", -1}
        };
        j2.bonus = false;
        j2.bonusCompleted = false;
    }

    static void endGame(player j1, player j2)
    {
        StreamWriter sw = new StreamWriter("Parties/" + DateTime.Now.ToString("yyMMdd_HHmmss") + ".json");
        string json = "{\n";

        json += "       \"parameters\": {\n";
        json += "           \"code\": \"groupe1-001\",\n";
        json += "           \"date\": \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"\n        },\n";
        json += "       \"players\": [\n";
        json += "           { \"id\": " + j1.id + ", \"pseudo\": \"" + j1.pseudo + "\" },\n";
        json += "           { \"id\": " + j2.id + ", \"pseudo\": \"" + j2.pseudo + "\" }\n";
        json += "       ],\n";



        json += "       \"rounds\": [\n";
        
        
        int index = 0;
        for(int i = 1; i <= 13; i++) 
        {
            json += "           {\n";
            json += "               \"id\": " + i + ",\n";
            json += "               \"results\": [\n";


            for(int y = 0; y < 2; y ++)
            {
                json += "                   {\n";
                json += "                       \"id_player\": " + historique[index].idPlayer + ",\n";
                json += "                       \"dice\": " + getJsonDice(index) + ",\n";
                json += "                       \"challenge\": \"" + historique[index].challengeChoise + "\",\n";
                json += "                       \"score\": " + historique[index].Score + "\n";
                
                if(y == 1)
                {
                    json += "                   }\n";
                }

                else
                {
                    json += "                   },\n";
                }
                index ++;
            }

            json += "               ]\n";

            if(i != 13)
            {
                json += "           },\n";
            }
            else
            {
                json += "           }\n";
            }
            
        }
        json += "       ],\n";
        json += "   \"final_result\": [\n";

        player[] tabPlayer = new player[2] {j1, j2};

        for(int i = 0; i < 2; i++)
        {
            json += "       {\n";
            json += "           \"id_player\": " + tabPlayer[i].id + ",\n";

            if(tabPlayer[i].bonus)
            {
                json += "           \"bonus\": 35,\n";
            }
            else
            {
                json += "           \"bonus\": 0,\n";
            }
            
            json += "           \"score\": " + tabPlayer[i].score + "\n";

            if(i != 1)
            {
                json += "       },\n";
            }
            else
            {
                json += "       }\n";
            }
        }
        json += "   ]\n}";
        
        sw.Write(json);
        sw.Close();
    }    

    static string getJsonDice(int index)
    {
        string str = "[";

        for(int i = 0; i < 4; i++)
        {
            str += historique[index].Des[i];
            str += ",";
        }

        str += historique[index].Des[4] + "]";

        return str;
    }

}
