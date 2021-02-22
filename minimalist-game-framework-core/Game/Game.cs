using System;
using System.Collections;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(320, 480);

    readonly Texture bubba = Engine.LoadTexture("char4R.png");
    readonly Texture Tplat1 = Engine.LoadTexture("plat.png");
    readonly Texture customPlatT = Engine.LoadTexture("plat1.png");
    readonly Texture bulletPic = Engine.LoadTexture("bullet.png");
    readonly Texture enemyPic = Engine.LoadTexture("enemy1.png");
    readonly Texture capText = Engine.LoadTexture("flyingCap.png");
    readonly Texture trampolineTex = Engine.LoadTexture("trampoline.png");
    readonly Texture shieldTex = Engine.LoadTexture("shield.png");
    readonly Texture bossEnemy = Engine.LoadTexture("bubbaEnemy.png");
    readonly Texture char1Zoom = Engine.LoadTexture("char1Zoom.png");
    readonly Texture char2Zoom = Engine.LoadTexture("char2Zoom.png");
    readonly Texture char3Zoom = Engine.LoadTexture("char3Zoom.png");
    readonly Texture char4Zoom = Engine.LoadTexture("char4Zoom.png");
    readonly Font font = Engine.LoadFont("FiraCode-Medium.ttf", pointSize: 20);
    readonly Font scoreFont = Engine.LoadFont("FiraCode-Medium.ttf", pointSize: 12);
    readonly Font pauseFont = Engine.LoadFont("FiraCode-Medium.ttf", pointSize: 8);
    float difficultyBasedOnChar = 1;
  

    readonly Texture background = Engine.LoadTexture("background.png");
    readonly Texture nightSky = Engine.LoadTexture("nightSky.png");
    readonly Texture endBackground = Engine.LoadTexture("endBackground.png");
    readonly Texture homeBackground = Engine.LoadTexture("homeBackground.png");

    readonly Texture pauseScreen = Engine.LoadTexture("PauseScreen.jpg");
    readonly Sound deadSound = Engine.LoadSound("Cat-sound-mp3.mp3");
    readonly Sound shootSound = Engine.LoadSound("shoot.mp3");
    readonly Sound jumpSound = Engine.LoadSound("jump.mp3");
    readonly Sound backgroundSound1 = Engine.LoadSound("bMusic1.mp3");
    readonly Music backgroundSound2 = Engine.LoadMusic("bMusic2.mp3");
    readonly Texture shieldForChar = Engine.LoadTexture("shieldForChar.png");


    
    List<Platform> platforms = new List<Platform>();
    ArrayList trampolines = new ArrayList();
    ArrayList flyingCaps = new ArrayList();
    ArrayList bullets = new ArrayList();
    ArrayList shields = new ArrayList();
    List<Enemy> enemies = new List<Enemy>();
    ArrayList brokenPlatforms = new ArrayList();
    Vector2 bck = new Vector2(0, 0);
    Boolean death = false;
    Boolean homeScreen = true;
    Boolean characterScreen = false;
    Boolean pause = false;
    Boolean bubbaActive = false;
    double difficulty = 1;
    int charSelect = 1;

    Vector2 plat1 = new Vector2(100, 300);

    Vector2 plat2 = new Vector2(200, 90);

    Vector2 plat3 = new Vector2(250, 30);

    Vector2 scoreVec = new Vector2(10, 10);

    Vector2 finalScoreVec = new Vector2(70, 210);

    Vector2 scoreBoardHeadVec = new Vector2(10, 225);
    Vector2 bossLocation = new Vector2(5, 60);


    int time = 0;


    private ScoreBoard sb;
    private int height;
    private int score;
    private int count;
    private Boolean jump;
    private Boolean compiled;
    private Boolean shieldOn;
    private Boolean alreadyUpdatedScores;

    private Boolean trampJump = false;
    private Boolean flying = false;
    private Boolean movingDown;
    private int downCount;
    private int lastPlatY;
    private Boolean shieldCooldown;
    Boolean bossDirectionRight = false;

    private int shieldCoolTimer;
    public Game()
    {
        sb = new ScoreBoard();
        lastPlatY = 470;
    }

    Character mainCharacter = new Character();

    public void Update()
    {
        difficulty = score * 0.0001 + 1;
        if(mainCharacter.getLocation().Y >= 480)
        {
            death = true;
        }
        if(death)
        {
            if (!alreadyUpdatedScores)
            {
                sb.modifyScoreBoard(score);
            }
            Engine.StopMusic(1);
            alreadyUpdatedScores = true;
            Engine.DrawTexture(endBackground, bck);
            Engine.DrawString(score.ToString(), finalScoreVec, Color.LightBlue, scoreFont);
            Engine.DrawString("High Scores:", scoreBoardHeadVec, Color.LawnGreen, scoreFont);
            Vector2 currentVec = new Vector2(scoreBoardHeadVec.X, scoreBoardHeadVec.Y + 15);
            for (int i = 1; i <= 10; i++)
            {
                Engine.DrawString(i + ": " + sb.getScore(i), currentVec, Color.Yellow, scoreFont);
                currentVec.Y += 15;
            }
            if (Engine.GetKeyHeld(Key.P))
            {
                int tempTexture = mainCharacter.getTextureNum();
                reset();
                mainCharacter.setTexture(tempTexture);
            }
            if(Engine.GetKeyHeld(Key.H))
            {
                reset();
                homeScreen = true;
                charSelect = 1;
            } 
        }
        else if(homeScreen)
        {
            Engine.DrawTexture(homeBackground, bck);
            Engine.DrawString("Emergency Key: Press 'P' to pause the game!", new Vector2(2, 430), Color.DarkRed, pauseFont);
            Engine.DrawString("Press the right arrow to go to the character selection screen.", new Vector2(2, 445), Color.Black, pauseFont);
            if(Engine.GetKeyHeld(Key.S))
            {
                homeScreen = false;
                alreadyUpdatedScores = false;
            }
            if(Engine.GetKeyHeld(Key.Right))
            {
                homeScreen = false;
                characterScreen = true;
            }
        }
        else if(characterScreen)
        {
            Engine.DrawTexture(background, bck);
            Engine.DrawString("Press 1, 2, 3, or 4 to switch your character!", new Vector2(2, 157), Color.Black, scoreFont);
            Engine.DrawString("If you are ready to play, press the 'S' key!", new Vector2(2, 420), Color.Black, scoreFont);
            Vector2 char1Loc = new Vector2(80, 200);
            Vector2 char2Loc = new Vector2(-20, 190);
            Vector2 char3Loc = new Vector2(40, 180);
            Vector2 char4Loc = new Vector2(90, 200);
            if(Engine.GetKeyDown(Key.NumRow1))
            {
                charSelect = 1;
            }
            if(Engine.GetKeyDown(Key.NumRow2))
            {
                charSelect = 2;
                difficultyBasedOnChar = (float)1.5;
            }
            if(Engine.GetKeyDown(Key.NumRow3))
            {
                charSelect = 3;
                difficultyBasedOnChar = (float)2;
            }
            if(Engine.GetKeyHeld(Key.NumRow4))
            {
                charSelect = 4;
                difficultyBasedOnChar = (float)2.5;
            }
            mainCharacter.setTexture(charSelect);
            if(charSelect == 2)
            {
                Engine.DrawTexture(char2Zoom, char2Loc);
            }
            else if(charSelect == 3)
            {
                Engine.DrawTexture(char3Zoom, char3Loc);
            }
            else if(charSelect == 4)
            {
                Engine.DrawTexture(char4Zoom, char4Loc);
            }
            else
            {
                Engine.DrawTexture(char1Zoom, char1Loc);
            }
            if(Engine.GetKeyHeld(Key.S))
            {
                characterScreen = false;
            }
        }
        else
        {
            //adding pause functionality to the game so that the player has the emergency pause 
            if(pause)
            {
                Engine.DrawTexture(pauseScreen, bck);
                if(Engine.GetKeyDown(Key.U))
                {
                    pause = false;
                }
            }
            else
            {
                if (height > score)
                {
                    score = height; //scoring algorithm 
                }
                
                Random random = new System.Random();


                //making sure the player is always moving down
                if (!jump && !movingDown)
                {
                    mainCharacter.setYLoc(5);
                    height -= 5;
                }


                charHittingEnemy();
                

                //need to have some cool down after the sheild period is over
                if(shieldCooldown)
                {
                    if(shieldCoolTimer >= 50)
                    {
                        shieldCoolTimer = 0;
                        shieldCooldown = false;
                    }
                    else
                    {
                        shieldCoolTimer++;
                    }
                }


                // making bubba have a night sky when he is active 
                if(bubbaActive)
                {
                    Engine.DrawTexture(nightSky, bck);
                }
                else
                {
                    Engine.DrawTexture(background, bck);
                }
                Engine.DrawTexture(mainCharacter.getCharTexture(), mainCharacter.getLocation());
                


                //if this is the first time the code is running these are the basic variables that need to be defined for the game to run
                if (!compiled && !death)
                {
                    lastPlatY = 470;
                    platforms.Add(new Platform(new Vector2(140, 465)));
                    for (int i = 0; i < 10; i++)
                    {

                        lastPlatY = random.Next(lastPlatY - 115, lastPlatY - 50);
                        Platform temp = new Platform(new Vector2(random.Next(0, 280), lastPlatY));

                        platforms.Add(temp);
                        Engine.PlayMusic(backgroundSound2);
                    }
                    compiled = true;
                }
                


                //making all the different game elements and drawing textures 
                foreach (Platform plat in platforms)
                {
                    Engine.DrawTexture(customPlatT, plat.getVector());
                }
                foreach (Enemy enemy in enemies)
                {
                    Engine.DrawTexture(enemyPic, enemy.getLocation());
                }
                foreach(Vector2 tramp in trampolines)
                {
                    Engine.DrawTexture(trampolineTex, tramp);
                }
                foreach (Vector2 cap in flyingCaps)
                {
                    Engine.DrawTexture(capText, cap);
                }
                foreach (Vector2 shield in shields)
                {
                    Engine.DrawTexture(shieldTex, shield);
                }
                Engine.DrawString(score.ToString(), scoreVec, Color.Purple, font);

                time++;

                makePlatforms();

                //making the character resopond to all the different keys it needs to 
                if(Engine.GetKeyHeld(Key.A))
                {
                    mainCharacter.respondToKey("A");
                }
                if(Engine.GetKeyHeld(Key.D))
                {
                    mainCharacter.respondToKey("D");
                }
                if(Engine.GetKeyDown(Key.Space))
                {
                    Engine.PlaySound(shootSound, false, 0);
                    bullets.Add(mainCharacter.shoot());
                }
                if(Engine.GetKeyDown(Key.P))
                {
                    pause = true;
                }
                jumping();
                shootingBullet();

        
                bubbaBoss();
                charHittingShield();
                if (shieldOn)
                {
                    Vector2 temp = mainCharacter.getLocation();
                    temp.X -= 3;
                    temp.Y -= 3;
                    Engine.DrawTexture(shieldForChar, temp);

                }
            }
        }
    }

    /// <summary>
    /// making sure the game is reset to its original fucntionality so that the player can polay again without having to "x" out of the game
    /// </summary>
    public void reset()
    {
        death = false;
        platforms = new List<Platform>();
        trampolines = new ArrayList();
        flyingCaps = new ArrayList();
        bullets = new ArrayList();
        shields = new ArrayList();
        enemies = new List<Enemy>();
        brokenPlatforms = new ArrayList();
        plat1 = new Vector2(100, 300);
        plat2 = new Vector2(200, 90);
        plat3 = new Vector2(250, 30);
        scoreVec = new Vector2(10, 10);
        time = 0;
        trampJump = false;
        flying = false;
        shieldOn = false;
        //lastPlatY = 470;
        mainCharacter = new Character();
        height = 0;
        score = 0;
        compiled = false;
        characterScreen = false;
        pause = false;
        bubbaActive = false;
    }


    /// <summary>
    /// making the bullets from the character and putting them in an array
    /// </summary>
    public void shootingBullet()
    {
        if (!death)
        {
            foreach (Vector2 bullet in bullets)
            {
                Engine.DrawTexture(bulletPic, bullet);
            }
            moveBulletUp();
            if (bullets.Count > 0 && enemies.Count > 0)
            {
                checkEnemyDead();
            }
        }
    }



    /// <summary>
    /// make sure the jump happens smoothly with all the different kinds of powerups 
    /// </summary>
    public void jumping()
    {
        if (!death)
        {
            if ((jump || hitting(mainCharacter.getLocation(), platforms)) && !trampJump && !flying)  // normal jump, count 25 up and then start coming back down with a height increase of 5 everytime
            {
                jump = true;
                if(hitting(mainCharacter.getLocation(), platforms) && jump)
                {
                    Engine.PlaySound(jumpSound, false, 0);

                }
                if (count < 25 && jump == true && !trampJump)
                {
                    double x;
                    x = mainCharacter.getLocation().Y - 5;
                    height += 5;
                    mainCharacter.newYPos((float)x);
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                else
                {
                    jump = false;
                    count = 0;
                }
            }

            if ((trampJump || hittingTramp(mainCharacter.getLocation(), trampolines)) && !flying && !jump) // trampoline jump, count to 20 and then start coming back down. Each itteration makes that character go up 15 pixels (3x than normal jump)
            {
                Console.WriteLine("inside tramp");
                trampJump = true;
                if (count < 20 && trampJump == true && !jump)
                {
                    double x;
                    x = mainCharacter.getLocation().Y - 15;
                    height += 15;
                    mainCharacter.newYPos((float)x);
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                else
                {
                    trampJump = false;
                    count = 0;
                }

            }

            if (flying || hittingCap(mainCharacter.getLocation(), flyingCaps) && !trampJump)  // flying, count to 50 --> long and fast jump to increase the user score | also adding that the user is invincivible duringthis time and doesnt get hurt
            {
                flying = true;
                
                if (count < 50 && flying == true && !jump)
                {
                    double x;
                    x = mainCharacter.getLocation().Y - 15;
                    height += 15;
                    Vector2 temp = mainCharacter.getLocation();
                    temp.X += 5;
                    temp.Y -= 10;
                    Engine.DrawTexture(capText, temp);
                    mainCharacter.newYPos((float)x);
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                else
                {
                    flying = false;
                    count = 0;
                }
            }



            //moving the game elements down every itteration based on the kind of jump jusrt accomplished 
            if (mainCharacter.getLocation().Y < 100)
            {
                

                if (jump)
                {
                    mainCharacter.setYLoc(5);
                    movePlatsDown(10);
                }
                
                else if (trampJump)
                {
                    mainCharacter.setYLoc(15);
                    movePlatsDown(20);
                }
                else if (flying)
                {
                    mainCharacter.setYLoc(15);
                    movePlatsDown(20);
                }
                else
                {
                    movePlatsDown(5);
                }

                downCount++;
                movingDown = true;
            }
            else
            {
                movingDown = false;
                downCount = 0;
            }
        }
    }






    /// <summary>
    /// moving all the bullets in the "bullets" array up so that they reach the enemy
    /// after the bullets reach a certain height make sure they get removed so that it doesnt take more memory
    /// </summary>
    public void moveBulletUp()
    {
        Vector2 temp = new Vector2();
        for (int i = 0; i < bullets.Count; i++)
        {
            temp = (Vector2)bullets[i];
            temp.Y = temp.Y - 15;
            bullets[i] = temp;
        }
        if (bullets.Count > 0)
        {
            temp = (Vector2)bullets[0];
            if (temp.Y < 0)
            {
                bullets.RemoveAt(0);
            }
        }
    }


    /// <summary>
    /// checking to make sure the enemy is dead from bullet, needed to make sure that the bullets work andf the enemy dies fromthem
    /// work throught the entire bullets and enemies array to make sure nothing is missed 
    /// </summary>
    public void checkEnemyDead()
    {
        if (!death) {
            for (int i = 0; i < bullets.Count; i++)
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    Vector2 currentBullet = new Vector2();
                    Vector2 currentEnemy = new Vector2();
                    if (bullets.Count > 0)
                    {
                        currentBullet = (Vector2)bullets[i];
                    }
                    if (enemies.Count > 0)
                    {
                        currentEnemy = (Vector2)enemies[j].getLocation();
                    }
                    if ((enemies.Count > 0 && bullets.Count > 0) && (currentBullet.Y - currentEnemy.Y < 40 && currentBullet.X - currentEnemy.X < 40 && currentBullet.X - currentEnemy.X > -9))
                    {
                        bullets.RemoveAt(i);
                        i--;
                        enemies.RemoveAt(j);
                        j--;
                    }
                }
            }
        }
    }

    /// <summary>
    /// moving all the game elements down x distance
    /// needs to be called when the player is above a certain height in the game
    /// delete the game elemenst after they reach a point in the cordinate plane to not use extra memeory
    /// </summary>
    /// <param name="distance"></param>
    public void movePlatsDown(int distance)
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            Platform temp = platforms[i];
            Vector2 tempVec = temp.getVector();
            tempVec.Y = tempVec.Y + distance;
            platforms[i] = new Platform(tempVec);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector2 temp = (Vector2)enemies[i].getLocation();
            temp.Y = temp.Y + distance;
            Enemy newEnemy = new Enemy();
            newEnemy.setLocation(temp);
            enemies[i] = newEnemy;
        }

        for (int i = 0; i < trampolines.Count; i++)
        {
            Vector2 temp = (Vector2)trampolines[i];
            temp.Y = temp.Y + distance;
            trampolines[i] = temp;
        }

        for (int i = 0; i < flyingCaps.Count; i++)
        {
            Vector2 temp = (Vector2)flyingCaps[i];
            temp.Y = temp.Y + distance;
            flyingCaps[i] = temp;
        }

        for (int i = 0; i < shields.Count; i++)
        {
            Vector2 temp = (Vector2)shields[i];
            temp.Y = temp.Y + distance;
            shields[i] = temp;
        }
    }



    /// <summary>
    /// make the random platforms
    /// on top of platforms start to add powerups and enemies
    /// ensure that there is no more than 1 thing per platform 
    /// Platforms should all be rechable by the player in all situations  
    /// </summary>
    public void makePlatforms()
    {
        Random random = new System.Random();

        Vector2 temp1 = platforms[0].getVector();
        if (temp1.Y > 490)
        {
            temp1 = platforms[platforms.Count - 1].getVector();
            int yLoc = (int)temp1.Y;
            int xLoc = (int)temp1.X;
            int newY = random.Next(yLoc - 120, yLoc - 50);
            int tempX = random.Next(0, 280);
            while (Math.Abs(xLoc - tempX) < 30)
            {
                tempX = random.Next(0, 280);
            }
            int newX = tempX;
            platforms.Add(new Platform(new Vector2(newX, newY)));
            platforms.RemoveAt(0);

            double enemyProb = random.Next(0, 100) / difficulty / difficultyBasedOnChar;
            double trampolineProb = random.Next(0, 100) * difficulty * difficultyBasedOnChar;
            double shieldProb = random.Next(0, 100) * difficulty * difficultyBasedOnChar;
            double capProb = random.Next(0, 100) * difficulty * difficultyBasedOnChar;
            Boolean trampPresent = false;
            Boolean enemyPresent = false;
            Boolean capPresent = false;

            if (trampolineProb < 20)
            {
                Vector2 trampolineTemp = new Vector2(newX, newY - 40);
                trampolines.Add(trampolineTemp);
                trampPresent = true;
            }

            if (capProb < 10 && trampPresent == false)
            {
                Vector2 capTemp = new Vector2(newX + 5, newY - 10);
                flyingCaps.Add(capTemp);
                capPresent = true;
            }

            if (enemyProb < 40 && yLoc - newY > 70 && !trampPresent && Math.Abs(xLoc - newX) < 60 && !capPresent)
            {
                Vector2 enemyTemp = new Vector2(newX, newY - 40);
                Enemy temp = new Enemy();
                temp.setLocation(enemyTemp);
                enemies.Add(temp);
                enemyPresent = true;
            }

            if (shieldProb < 20 && !trampPresent && !enemyPresent && !capPresent)
            {
                Vector2 shieldTemp = new Vector2(newX, newY - 40);
                shields.Add(shieldTemp);
            }

            trampPresent = false;
            enemyPresent = false;
            capPresent = false;


        }
    }



    /// <summary>
    /// Making bubba appear in the game after a certain time based on score so that the game becomes harder and more distracting 
    /// 
    /// if player has shield make the shield disapeewar and not kill the player
    /// </summary>
    public void bubbaBoss()
    {
        if (score > 4000 && score % 3000 > 0 && score % 3000 < 2000)
        {
            bubbaActive = true;
            if (bossLocation.X > 310)
            {
                bossDirectionRight = false;
            }

            if (bossLocation.X < 10)
            {
                bossDirectionRight = true;
            }

            if (bossDirectionRight == true)
            {
                bossLocation.X += 3;
            }
            else
            {
                bossLocation.X -= 3;
            }
            Engine.DrawTexture(bossEnemy, bossLocation);

            if (!flying && !trampJump && !shieldOn && Math.Abs(bossLocation.X - mainCharacter.getLocation().X) < 50 && Math.Abs(bossLocation.X - mainCharacter.getLocation().X) > 0 && Math.Abs(bossLocation.Y - mainCharacter.getLocation().Y) > 0 && Math.Abs(bossLocation.Y - mainCharacter.getLocation().Y) < 50)
            {
                death = true;
                Console.WriteLine("CharPosition x = " + mainCharacter.getLocation().X + " y = " + mainCharacter.getLocation().Y);
                Console.WriteLine("Bubba Pos x = " + bossLocation.X + " y = " + bossLocation.Y);
            }

            if (shieldOn && !flying && !trampJump && Math.Abs(bossLocation.X - mainCharacter.getLocation().X) < 50 && Math.Abs(bossLocation.X - mainCharacter.getLocation().X) > 0 && Math.Abs(bossLocation.Y - mainCharacter.getLocation().Y) > 0 && Math.Abs(bossLocation.Y - mainCharacter.getLocation().Y) < 50)
            {
                score += 1000;
                shieldOn = false;
            }
        }
        else
        {
            bubbaActive = false;
        }
    }



    /// <summary>
    /// check if the character is hitting the game and return a boolean so that it can be used in the jumping algorithm
    /// Have offsets of 40 becasue of size
    /// </summary>
    public Boolean hitting(Vector2 charLocation, List<Platform> platforms)
    {
        if (!death || !trampJump)
        {

            foreach (Platform platform in platforms)
            {
                if (Math.Abs(charLocation.X - platform.getVector().X) <= 40 && charLocation.Y - platform.getVector().Y <= -35 && charLocation.Y - platform.getVector().Y >= -40)
                {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// functionality for after the character the character has hit the trampoline
    /// check if death are not true so that other functionality in the game is not impeded 
    /// return true so that the ciode can use it in the jumping
    /// </summary>
    public Boolean hittingTramp(Vector2 charLocation, ArrayList tramps)
    {
        if (!death)
        {
            {
                foreach (Vector2 platform in tramps)
                {
                    if (Math.Abs(charLocation.X - platform.X) <= 40 && Math.Abs(charLocation.Y - platform.Y) <= 29)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }



    /// <summary>
    /// functionality for after the character the character has hit the cap
    /// check if deatrh and trampjump are not true so that other functionality in the game is not impeded 
    /// return true so that the ciode can use it in the jumping
    /// </summary>
    
    public Boolean hittingCap(Vector2 charLocation, ArrayList caps)
    {
        int i = 0;
        if (!death || !trampJump)
        {
            {
                foreach (Vector2 platform in caps)
                {

                    i++;
                    if (Math.Abs(charLocation.X - platform.X) <= 40 && Math.Abs(charLocation.Y - platform.Y) <= 40 && !jump)
                    {
                        Console.WriteLine("hit cap");
                        caps.RemoveAt(i-1);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// implementing funcationailty for after character has been hit by enemy and game needs to start ending
    /// But if the character has a shield, only remove the shield and enemy but let the enemy live
    /// </summary>
    public void charHittingEnemy()
    {
        int charX = (int)mainCharacter.getLocation().X;
        int charY = (int)mainCharacter.getLocation().Y;
        Boolean removed = false;
        int enemyNum = -1;

        if (!trampJump && !flying)
        {
            
            foreach (Enemy enemy in enemies)
            {
                int enemyX = (int)enemy.getLocation().X;
                int enemyY = (int)enemy.getLocation().Y;

                if (Math.Abs(enemyX - charX) < 40 && Math.Abs(enemyY - charY) < 10)
                {
                    if (!shieldOn && !shieldCooldown)
                    {
                        charHitEnemy();
                        death = true;
                        jump = false;
                   
                    }
                    else
                    {
                        shieldOn = false;
                        shieldCooldown = true;
                        removed = true;
                    }
                }
                enemyNum++;
            }
        }
        if (removed)
        {
            enemies.RemoveAt(enemyNum);
        }

    }


    /// <summary>
    /// functionality for after the character hit the enemy, at this point the starts to end and go to to the end screen
    /// </summary>
    public void charHitEnemy()
    {
        Engine.PlaySound(deadSound, false, 0);
        while (mainCharacter.getLocation().Y < Resolution.Y)
        {
            int charCurrentY = (int)mainCharacter.getLocation().Y;

            mainCharacter.setYLoc(charCurrentY - 10);



            break;

        }
    }



    /// <summary>
    /// check if the player is hitting the shield, if yes --> provide the character with the shield for further use
    /// </summary>
    public void charHittingShield()
    {
        int charX = (int)mainCharacter.getLocation().X;
        int charY = (int)mainCharacter.getLocation().Y;
        if (!death)
        {
            Vector2 currentShield;
            for(int i = 0; i < shields.Count; i++)
            {
                currentShield = (Vector2)shields[i];
                if (Math.Abs(charX - currentShield.X) <= 40 && charY - currentShield.Y <= -30 && charY - currentShield.Y >= -40 && !shieldOn)
                {
                    shieldOn = true;
                    Engine.DrawTexture(shieldForChar, mainCharacter.getLocation());
                    Console.WriteLine("has shield");
                    shields.RemoveAt(i);
                    return;
                    
                }
            }
        }
    }
}