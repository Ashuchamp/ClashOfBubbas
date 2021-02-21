using System;
using System.Collections;
using System.Collections.Generic;

class Game
{
    public static readonly string Title = "Minimalist Game Framework";
    public static readonly Vector2 Resolution = new Vector2(320, 480);

    Texture charRight = Engine.LoadTexture("charR.png");
    readonly Texture charLeft = Engine.LoadTexture("charL.png");
    readonly Texture Tplat1 = Engine.LoadTexture("plat.png");
    readonly Texture customPlatT = Engine.LoadTexture("plat1.png");
    readonly Texture bulletPic = Engine.LoadTexture("bullet.png");
    readonly Texture enemyPic = Engine.LoadTexture("enemy.png");
    readonly Texture capText = Engine.LoadTexture("flyingCap.png");
    readonly Texture trampolineTex = Engine.LoadTexture("trampoline.png");
    readonly Texture shieldTex = Engine.LoadTexture("shield.png");
    readonly Font font = Engine.LoadFont("FiraCode-Medium.ttf", pointSize: 20);
    //readonly Texture Tplat2 = Engine.LoadTexture("plat.png");
    //readonly Texture Tplat3 = Engine.LoadTexture("plat.png");

    readonly Texture background = Engine.LoadTexture("background.png");
    //readonly Texture endBackground = Engine.LoadTexture("endBackground.png");
    readonly Texture homeBackground = Engine.LoadTexture("homeBackground.png");
    readonly Sound deadSound = Engine.LoadSound("Cat-sound-mp3.mp3");

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

    Vector2 plat1 = new Vector2(100, 300);

    Vector2 plat2 = new Vector2(200, 90);

    Vector2 plat3 = new Vector2(250, 30);

    Vector2 scoreVec = new Vector2(10, 10);

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
    private int shieldCoolTimer;
    public Game()
    {
        sb = new ScoreBoard();
        lastPlatY = 470;
    }

    Character mainCharacter = new Character();

    public void Update()
    {

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
            alreadyUpdatedScores = true;
            //Engine.DrawTexture(endBackground, bck);
            if (Engine.GetKeyHeld(Key.P))
            {
                death = false;
            }
            if(Engine.GetKeyHeld(Key.H))
            {
                death = false;
                homeScreen = true;
            } 
        }
        else if(homeScreen)
        {
            Engine.DrawTexture(homeBackground, bck);
            if(Engine.GetKeyHeld(Key.S))
            {
                homeScreen = false;
                height = 0;
                score = 0;
                alreadyUpdatedScores = false;
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
            charHittingShield();
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

            Engine.DrawTexture(background, bck);
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
            if(Engine.GetKeyHeld(Key.Space))
            {
                bullets.Add(mainCharacter.shoot());
            }
            jumping();
            shootingBullet();
            //}

        
            //breakPlatform();
        }
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

            else if ((trampJump || hittingTramp(mainCharacter.getLocation(), trampolines)) && !flying)
            {
                Console.WriteLine("inside tramp");
                trampJump = true;
                if (count < 25 && trampJump == true && !jump)
                {
                    double x;
                    x = mainCharacter.getLocation().Y - 10;
                    height += 10;
                    mainCharacter.newYPos((float)x);
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                else //if (count > 25 && trampJump == false)
                {
                    trampJump = false;
                    count = 0;
                }

            }

            else if (flying || hittingCap(mainCharacter.getLocation(), flyingCaps))
            {
                flying = true;
                if (count < 50 && flying == true && !jump)
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
                    mainCharacter.setYLoc(10);
                    movePlatsDown(15);
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
            int newY = random.Next(yLoc - 125, yLoc - 50);
            int tempX = random.Next(0, 280);
            while(Math.Abs(xLoc - tempX) < 30)
            {
                tempX = random.Next(0, 280);
            }
            int newX = tempX;
            platforms.Add(new Platform(new Vector2(newX, newY)));
            platforms.RemoveAt(0);

            int enemyProb = random.Next(0, 100);
            int trampolineProb = random.Next(0, 100);
            int shieldProb = random.Next(0, 100);
            int capProb = random.Next(0, 100);
            Boolean trampPresent = false;
            Boolean enemyPresent = false;
            Boolean capPresent = false;

            if (trampolineProb < 20)
            {
                Vector2 trampolineTemp = new Vector2(newX, newY - 40);
                trampolines.Add(trampolineTemp);
                trampPresent = true;
            }

            if (capProb < 10)
            {
                Vector2 capTemp = new Vector2(newX, newY - 40);
                flyingCaps.Add(capTemp);
                capPresent = true;
            }

            if (enemyProb < 20 && yLoc - newY > 70 && !trampPresent && Math.Abs(xLoc - newX) < 60 && !capPresent)//  && yLoc - newY < -40)// && !trampPresent)
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

    public Boolean hitting(Vector2 charLocation, List<Platform> platforms)
    {
        if (!death || !trampJump)
        {

            foreach (Platform platform in platforms)
            {
                if (Math.Abs(charLocation.X - platform.getVector().X) <= 40 && charLocation.Y - platform.getVector().Y <= -30 && charLocation.Y - platform.getVector().Y >= -40)
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
        if (!death)
        {
            {
                foreach (Vector2 platform in caps)
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

    public void charHittingEnemy()
    {
        int charX = (int)mainCharacter.getLocation().X;
        int charY = (int)mainCharacter.getLocation().Y;

        if (!trampJump)
        {
            foreach (Enemy enemy in enemies)
            {
                int enemyX = (int)enemy.getLocation().X;
                int enemyY = (int)enemy.getLocation().Y;

                if (Math.Abs(enemyX - charX) < 20 && Math.Abs(enemyY - charY) < 20)
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
                    }
                }
            }
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



            break;

        }
    }

    public void charHittingShield()
    {
        int charX = (int)mainCharacter.getLocation().X;
        int charY = (int)mainCharacter.getLocation().Y;
        if (!death || !trampJump)
        {
            Vector2 currentShield;
            for(int i = 0; i < shields.Count; i++)
            {
                currentShield = (Vector2)shields[i];
                if (Math.Abs(charX - currentShield.X) <= 40 && charY - currentShield.Y <= -30 && charY - currentShield.Y >= -40 && !shieldOn)
                {
                    shieldOn = true;
                    shields.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}