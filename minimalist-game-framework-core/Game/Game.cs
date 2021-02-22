using System;
using System.Collections;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(320, 480);
    //Texture charRight = Engine.LoadTexture("charR.png");

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
    //readonly Texture Tplat2 = Engine.LoadTexture("plat.png");
    //readonly Texture Tplat3 = Engine.LoadTexture("plat.png");

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


    //Vector2 charLocation = new Vector2(145, 440);
    //Vector2 platLocation = new Vector2(100, 300);
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
    //    public void plats()
    //    { 
    //        Random random = new System.Random();
    //        for (int i = 0; i < 5; i++)
    //        {
    //            Vector2 temp = new Vector2(random.Next(0, 280), random.Next(50, 400));
    //            platforms.Add(temp);
    //            Engine.DrawTexture(plat1, temp);
    //
    //        }
    //    }

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
            //Engine.StopMusic(1);
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
                    score = height;
                }
                //platforms.Add(plat1);
                //platforms.Add(plat2);
                //platforms.Add(plat3);
                Random random = new System.Random();

                if (!jump && !movingDown)
                {
                    mainCharacter.setYLoc(5);
                    height -= 5;
                }
                charHittingEnemy();
                //charHittingShield();
                //charLocation.Y += 5;

                if(shieldCooldown)
                {
                    if(shieldCoolTimer >= 250)
                    {
                        shieldCoolTimer = 0;
                        shieldCooldown = false; //need to add different texture!!!!!!!!!!!!!-----------------------------!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    }
                    else
                    {
                        shieldCoolTimer++;
                    }
                }
                if(bubbaActive)
                {
                    Engine.DrawTexture(nightSky, bck);
                }
                else
                {
                    Engine.DrawTexture(background, bck);
                }
                Engine.DrawTexture(mainCharacter.getCharTexture(), mainCharacter.getLocation());
                //Engine.DrawTexture(Tplat1, plat1);
                //Engine.DrawTexture(Tplat1, plat2);
                //Engine.DrawTexture(Tplat1, plat3);

                if (!compiled && !death)
                {
                    lastPlatY = 470;
                    platforms.Add(new Platform(new Vector2(140, 465)));
                    for (int i = 0; i < 10; i++)
                    {

                        //int enemyProb = random.Next(0, 100);
                        lastPlatY = random.Next(lastPlatY - 115, lastPlatY - 50);
                        Platform temp = new Platform(new Vector2(random.Next(0, 280), lastPlatY));

                        platforms.Add(temp);
                        Engine.PlayMusic(backgroundSound2);
                        //Engine.DrawTexture(customPlatT, temp);
                    }
                    compiled = true;
                }

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
                //charActions();
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
                //}

        
                //breakPlatform();
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

    public void makeBrokenPlatform()
    {
        Random random = new System.Random();
        for (int i = 0; i < platforms.Count; i++)
        {
            if (random.Next(0, 100) < 80)
            {
                brokenPlatforms.Add(platforms[i]);
                Vector2 temp = new Vector2();
                temp = platforms[1].getVector();

            }
        }
    }

    public void jumping()
    {
        if (!death)
        {
            if ((jump || hitting(mainCharacter.getLocation(), platforms)) && !trampJump && !flying)
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

            if ((trampJump || hittingTramp(mainCharacter.getLocation(), trampolines)) && !flying && !jump)
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
                else// if (count > 25 && trampJump == false)
                {
                    trampJump = false;
                    count = 0;
                }

            }

            if (flying || hittingCap(mainCharacter.getLocation(), flyingCaps) && !trampJump)
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




            if (mainCharacter.getLocation().Y < 100)
            {
                //if(downCount < 25)
                //{

                if (jump)
                {
                    mainCharacter.setYLoc(5);
                    movePlatsDown(10);
                }
                /*else
                {
                    //mainCharacter.setYLoc(15);
                    movePlatsDown(10);
                } */
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
                    //mainCharacter.setYLoc(20);
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

    /*public void charActions()
    {

        if (Engine.GetKeyHeld(Key.A)) // && charLocation.X > 0)
        {
            charRight = Engine.LoadTexture("charL.png");

            if (charLocation.X < 0)
            {
                charLocation.X = 300;
            }
            charLocation.X = charLocation.X - 5;
        }
        if (Engine.GetKeyHeld(Key.D)) //&& charLocation.X < 290)
        {
            charRight = Engine.LoadTexture("charR.png");

            if (charLocation.X > 300)
            {
                charLocation.X = 0;
            }
            charLocation.X = charLocation.X + 5;
            Console.WriteLine("D pressed");
        }
        if (Engine.GetKeyHeld(Key.S))
        {
            charLocation.Y = charLocation.Y + 5;
        }
        if (Engine.GetKeyHeld(Key.W))
        {
            charLocation.Y = charLocation.Y - 10;
        }
        if (Engine.GetKeyDown(Key.Space))
        {
            charRight = Engine.LoadTexture("shoot.png");
            Vector2 temp = new Vector2();
            temp = charLocation;
            temp.Y = temp.Y - 2;
            temp.X = temp.X + 15;
            bullets.Add(temp);
        }
    }*/

    public void breakPlatform()
    {
        Random random = new System.Random();
        for (int i = 0; i < brokenPlatforms.Count; i++)
        {
            Vector2 platformV = platforms[i].getVector();
            if ((Math.Abs(mainCharacter.getLocation().X - platformV.X) <= 40 && Math.Abs(mainCharacter.getLocation().Y - platformV.Y) <= 29) && random.Next(0, 100) > 80)
            {
                brokenPlatforms.RemoveAt(i);
                return;
            }
        }
    }

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
                    //if (enemies.Count > 0 && bullets.Count > 0) 
                    // {
                    if ((enemies.Count > 0 && bullets.Count > 0) && (currentBullet.Y - currentEnemy.Y < 40 && currentBullet.X - currentEnemy.X < 40 && currentBullet.X - currentEnemy.X > -9))
                    {
                        bullets.RemoveAt(i);
                        i--;
                        enemies.RemoveAt(j);
                        j--;
                    }
                    // }
                }
            }
        }
    }

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

            if (enemyProb < 40 && yLoc - newY > 70 && !trampPresent && Math.Abs(xLoc - newX) < 60 && !capPresent)//  && yLoc - newY < -40)// && !trampPresent)
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

    public void bubbaBoss()
    {
        if (score > 4000 && score % 3000 > 0 && score % 3000 < 2000)
        {
            bubbaActive = true;
            if (bossLocation.X > 310)
            {
                bossDirectionRight = false;
                //System.Threading.Thread.Sleep(10);
            }

            if (bossLocation.X < 10)
            {
                bossDirectionRight = true;
                //System.Threading.Thread.Sleep(10);
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
                //Engine.StopMusic(1);
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

    //still temporarily needed for trampolines
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
                       // Engine.StopMusic(1);
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
        //return false;

    }

    public void charHitEnemy()
    {
        //Sound sound = new Sound("Cat-sound-mp3.mp3");
        Engine.PlaySound(deadSound, false, 0);
        while (mainCharacter.getLocation().Y < Resolution.Y)
        {
            //int charCurrentX = (int)mainCharacter.getLocation().X;
            int charCurrentY = (int)mainCharacter.getLocation().Y;

            mainCharacter.setYLoc(charCurrentY - 10);
            //Engine.StopMusic(1);



            break;

        }
    }

    public void charHittingShield()
    {
        int charX = (int)mainCharacter.getLocation().X;
        int charY = (int)mainCharacter.getLocation().Y;
        if (!death)// || !trampJump)
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
                    //shieldOn = true;
                    return;
                    
                }
            }
        }
    }
}