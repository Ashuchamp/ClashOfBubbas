using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Enemy
{
    Boolean death;
    Vector2 location;
    Boolean enemyType;
    Texture enemy1 = Engine.LoadTexture("enemy1.png");
    Texture enemy2 = Engine.LoadTexture("enemy2.png");
    int health;

    public void makeEnemy(Vector2 location)
    {
        this.location = location;
        Random random = new System.Random();
        if(random.Next(0, 100) > 50)
        {
            enemyType = true;
            health = 1;
            Engine.DrawTexture(enemy1, location);
        }
        else
        {
            enemyType = false;
            health = 2;
            Engine.DrawTexture(enemy2, location);

        }
        death = false;
    }

    public Vector2 getLocation()
    {
        return location;
    }

    public void setLocation(Vector2 location)
    {
        this.location = location;
    }

    public void enemy2Hit()
    {
        Texture enemy2 = Engine.LoadTexture("enemy1.png");
        health--;
    }

    public Boolean isDead()
    {
        if (health == 0)
        {
            return true;
        }
        return false;
    }
}