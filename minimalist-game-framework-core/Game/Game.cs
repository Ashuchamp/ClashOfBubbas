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
    readonly Texture trampolineTex = Engine.LoadTexture("trampoline.png");
    readonly Font font = Engine.LoadFont("FiraCode-Medium.ttf", pointSize: 20);
    //readonly Texture Tplat2 = Engine.LoadTexture("plat.png");
    //readonly Texture Tplat3 = Engine.LoadTexture("plat.png");

    readonly Texture background = Engine.LoadTexture("background.jpg");

    //Vector2 charLocation = new Vector2(145, 440);
    //Vector2 platLocation = new Vector2(100, 300);
    List<Platform> platforms = new List<Platform>();
    ArrayList trampolines = new ArrayList();
    ArrayList bullets = new ArrayList();
    List<Enemy> enemies = new List<Enemy>();
    ArrayList brokenPlatforms = new ArrayList();
    Vector2 bck = new Vector2(0, 0);

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

    private int height;
    private int score;
    private int count;
    private Boolean jump;
    private Boolean compiled;
    private Boolean movingDown;
    private int downCount;
    private int lastPlatY;
    public Game()
    {
        lastPlatY = 470;
        height = 470;
    }

    Character mainCharacter = new Character();

    public void Update()
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
        //charLocation.Y += 5;

        Engine.DrawTexture(background, bck);
        Engine.DrawTexture(mainCharacter.getCharTexture(), mainCharacter.getLocation());
        //Engine.DrawTexture(Tplat1, plat1);
        //Engine.DrawTexture(Tplat1, plat2);
        //Engine.DrawTexture(Tplat1, plat3);

        if (!compiled)
        {
            lastPlatY = 470;
            platforms.Add(new Platform(new Vector2(140, 465)));
            for (int i = 0; i < 10; i++)
            {

                //int enemyProb = random.Next(0, 100);
                lastPlatY = random.Next(lastPlatY - 125, lastPlatY - 50);
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

    public void shootingBullet()
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
        if (jump || hitting(mainCharacter.getLocation(), platforms))
        {
            jump = true;
            if (count < 25 && jump == true)
            {
                count++;
                double x;
                if (hittingTramp(mainCharacter.getLocation(), trampolines))
                {
                    x = mainCharacter.getLocation().Y - 20;
                    height += 20;
                }
                else
                {
                    x = mainCharacter.getLocation().Y - 5;
                    height += 5;
                }
                mainCharacter.newYPos((float)x);
                System.Threading.Thread.Sleep(10);
            }
            else
            {
                jump = false;
                count = 0;
                //
            }
        }
        if (mainCharacter.getLocation().Y < 100)
        {
            //if(downCount < 25)
            //{
            movePlatsDown();
            if (jump)
            {
                mainCharacter.setYLoc(5);
            }
            else
            {
                mainCharacter.setYLoc(15);
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
                if ((enemies.Count > 0 && bullets.Count > 0) && (currentBullet.Y - currentEnemy.Y < 40 && currentBullet.X - currentEnemy.X < 40 && currentEnemy.X - currentBullet.X > -9))
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

    public void movePlatsDown()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            Platform temp = platforms[i];
            Vector2 tempVec = temp.getVector();
            tempVec.Y = tempVec.Y + 10;
            platforms[i] = new Platform(tempVec);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector2 temp = (Vector2)enemies[i].getLocation();
            temp.Y = temp.Y + 10;
            Enemy newEnemy = new Enemy();
            newEnemy.setLocation(temp);
            enemies[i] = newEnemy;
        }

        for (int i = 0; i < trampolines.Count; i++)
        {
            Vector2 temp = (Vector2)trampolines[i];
            temp.Y = temp.Y + 10;
            trampolines[i] = temp;
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
            int newY = random.Next(yLoc - 125, yLoc - 50);
            int newX = random.Next(0, 280);
            platforms.Add(new Platform(new Vector2(newX, newY)));
            platforms.RemoveAt(0);

            int enemyProb = random.Next(0, 100);
            int trampolineProb = random.Next(0, 100);

            if (enemyProb < 20)
            {
                Vector2 enemyTemp = new Vector2(newX, newY - 40);
                Enemy temp = new Enemy();
                temp.setLocation(enemyTemp);
                enemies.Add(temp);
            }

            if(trampolineProb < 10)
            {
                Vector2 trampolineTemp = new Vector2(newX, newY - 40);
                trampolines.Add(trampolineTemp);
            }
        }
    }

    public Boolean hitting(Vector2 charLocation, List<Platform> platforms)
    {
        foreach (Platform platform in platforms)
        {
            if (Math.Abs(charLocation.X - platform.getVector().X) <= 30 && charLocation.Y - platform.getVector().Y <= -30 && charLocation.Y - platform.getVector().Y >= -40)
            {
                return true;
            }
        }
        return false;
    }

    //still temporarily needed for trampolines
    public Boolean hittingTramp(Vector2 charLocation, ArrayList platforms)
    {
        foreach (Vector2 platform in platforms)
        {
            if (Math.Abs(charLocation.X - platform.X) <= 40 && Math.Abs(charLocation.Y - platform.Y) <= 29)
            {
                return true;
            }
        }

        return false;
    }

}