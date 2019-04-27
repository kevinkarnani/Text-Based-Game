﻿using System;

namespace TBD_TBG
{
    public class Combat
    {
        /*
         * TODO:
         * add in evasion (chance to hit)
         * add in dodge for player
         * add in heavy attack for enemy
         * add in dodge for enemy
         */
        private Enemy enemy;
        public double initialEvasion = Player.playerStats.Evasion;
        public bool preparingAttack = false;
        static readonly string battleColor = "yellow";
        static readonly string choiceColor = "cyan";

        public Combat(Enemy _enem)
        {
            enemy = _enem; 
        }

        //TODO: 
        public void StartCombatLoop()
        {
            //header of the battle
            Utility.Write(">>>>>----------> BATTLE START <----------<<<<<", battleColor);
            Utility.Write("You face a " + enemy.name + "!", battleColor);
            Utility.Write("Description: " + enemy.description, battleColor);
            Console.WriteLine();

            int turnNumber = 1;
            var pressAnyKey ="";
            //start loop until someone dies
            while (Player.playerStats.CurrentHP >= 0 && enemy.enemyStats.CurrentHP > 0)
            {              
                Utility.Write("=====TURN #" + turnNumber + "=====", battleColor);
                //determine who goes first
                if (Player.playerStats.Agility > enemy.enemyStats.Agility) //the player goes first
                {
                    PlayerTurn();
                    if (enemy.enemyStats.CurrentHP == 0) //if you kill the enemy before they can attack back
                    {
                        break;
                    }
                    pressAnyKey = Console.ReadLine();

                    EnemyTurn();
                    pressAnyKey = Console.ReadLine();
                }
                else //the enemy goes first
                {
                    EnemyTurn();
                    if (Player.playerStats.CurrentHP == 0) //if the enemy kills you before you can attack back
                    {
                        break;
                    }
                    pressAnyKey = Console.ReadLine();

                    PlayerTurn();
                    pressAnyKey = Console.ReadLine();
                }
                turnNumber++;
            }
            //determines who won the battle
            if (enemy.enemyStats.CurrentHP == 0) //you win
            {
                Utility.Write("You defeated the " + enemy.name + "!", battleColor);
                Utility.Write(">>>>>----------> BATTLE FINISH <----------<<<<<", battleColor);
            }
            else //you lose
            {
                Utility.Write("You lost to the " +  enemy.name + "!", battleColor);
                Utility.Write(">>>>>----------> BATTLE FINISH <----------<<<<<", battleColor);
                Utility.Write("GAME OVER");
                Game.End();
            }
        }
        private void PlayerTurn()
        {
            //PLAYER TURN
            Utility.Write("---PLAYER TURN---", battleColor);
            if (preparingAttack)
            {
                if (Player.CheckIfHit()) //hit
                {
                    Player.HeavyAttack(enemy);
                    Utility.Write("You hit for " + Player.playerStats.HeavyAttack + " damage!", battleColor);
                }
                else //miss
                {
                    Utility.Write(enemy.name + " evaded your attack!", battleColor);
                }                
                preparingAttack = false;
            }
            else
            {
                //TODO: Make sure this input is a number between 1 and 3                
                Utility.Write("Options:", choiceColor);
                Utility.Write("1.) Light attack", choiceColor);
                Utility.Write("2.) Heavy attack", choiceColor);
                Utility.Write("3.) Dodge", choiceColor);
                //Utility.Write("4.) Display Stats");
                //Utility.Write("4.) Use an item");
                //Utility.Write("5.) Flee?");
                string attackChoice = Console.ReadLine();

                //make attack choice
                switch (attackChoice)
                {
                    case "1":
                        if (Player.CheckIfHit()) //hit
                        {
                            Player.LightAttack(enemy);
                            Utility.Write("You hit for " + Player.playerStats.Attack + " damage!", battleColor);
                        }
                        else //miss
                        {
                            Utility.Write(enemy.name + " evaded your attack!", battleColor);
                        }
                        
                        break;
                    case "2":
                        Utility.Write("You charge up for a powerful attack...", battleColor);
                        preparingAttack = true;
                        break;
                        //case "3":
                        //  Player.playerStats.Evasion *= 2;
                        //break;
                }                
            }
            //display health of enemy            
            Utility.Write("Enemy HP:" + enemy.enemyStats.CurrentHP + "/" + enemy.enemyStats.MaxHP, battleColor);
            //Console.WriteLine();
        }
        private void EnemyTurn()
        {
            //ENEMY TURN              
            Utility.Write("---ENEMY TURN---", battleColor);
            switch (enemy.ChooseRandomAttack())
            {
                case "lightAttack":
                    if (Player.CheckIfHit()) //hit
                    {
                        enemy.LightAttack();
                        Utility.Write(enemy.name + " hit for " + enemy.enemyStats.Attack + " damage!", battleColor);
                    }
                    else //miss
                    {
                        Utility.Write("You evaded their attack!", battleColor);
                    }
                    
                    break;
                    /*case "heavyAttack":
                        Utility.Write("You charge up for a powerful attack...", battleColor);
                        preparingAttack = true;
                        break;
                    //case "dodge":
                        //  Player.playerStats.Evasion *= 2;
                        //break;*/
            }
            //display health of player         
            Utility.Write("Player HP:" + Player.playerStats.CurrentHP + "/" + Player.playerStats.MaxHP, battleColor);
            //Console.WriteLine();
        }

    }

}
