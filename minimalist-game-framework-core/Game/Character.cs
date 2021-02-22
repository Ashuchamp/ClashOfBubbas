using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Character
{
    Texture charTexture = Engine.LoadTexture("bubba.png");
    //readonly Texture charLeft = Engine.LoadTexture("charL.png");
    //readonly Texture shoot = Engine.LoadTexture("shoot.png");
    private Vector2 charLocation;
    private Boolean powerupActivated;
    //private Boolean jump = false;
    //private ArrayList powerups = new ArrayList();

    public Vector2 getLocation()
    {
        return charLocation;
    } 
    public void setXLoc(float xIncrement)
    {
        charLocation.X += xIncrement;
    }
    public void setYLoc(float yIncrement)
    {
        charLocation.Y += yIncrement;
    }
    public void newYPos(float newY)
    {
        charLocation.Y = newY;
    }
    public Texture getCharTexture()
    {
        return charTexture;
    }
    /*public Boolean getJump()
    {
        return jump;
    }
    public void setJump(Boolean newJump)
    {
        jump = newJump;
    }*/
    public Character()
    {
        charLocation = new Vector2(145, 340);
        powerupActivated = false;
    }
    public void respondToKey(String keyName)
    {
        if (keyName == "A") // && charLocation.X > 0)
        {
            charTexture = Engine.LoadTexture("charL.png");

            if (charLocation.X < 0)
            {
                charLocation.X = 300;
            }
            charLocation.X = charLocation.X - 5;
        }
        if (keyName == "D") //&& charLocation.X < 290)
        {
            charTexture = Engine.LoadTexture("charR.png");

            if (charLocation.X > 300)
            {
                charLocation.X = 0;
            }
            charLocation.X = charLocation.X + 5;
        }
        /*if (keyName == "S")
        {
            charLocation.Y = charLocation.Y + 5;
        }

    */
        if (keyName == "W")
        {
            charLocation.Y = charLocation.Y - 10;
        }
    }
    public Vector2 shoot()
    {
        charTexture = Engine.LoadTexture("shoot.png");
        Vector2 temp = new Vector2();
        temp = charLocation;
        temp.Y = temp.Y - 2;
        temp.X = temp.X + 15;
        return temp;
    }

    public void addPowerups(Powerup powerup)
    {
        //get location from char and location of powerups
        if (charLocation.X == powerup.getLocation().X && charLocation.Y == powerup.getLocation().Y)
        {
            //if match then add powerup to arraylist
            //powerups.Add(powerup);
            //hold calling this method until powerup = false?
            if (!powerupActivated)
            {
                powerupActivated = true;
                activatePowerup(powerup.getName());
            }
            else
            {
                //callback function?
                while (powerupActivated)
                {
                    Console.WriteLine("Powerup active, so can't activate new powerup.");
                }
                activatePowerup(powerup.getName());
                //keep trying to activate powerup code
            }
        }
    }

    public void activatePowerup(String powerUpName)
    {
        //update textures and physics
        charTexture = Engine.LoadTexture("char" + powerUpName + ".png");
        int changeY = 0;
        switch (powerUpName)
        {
            case "trampoline":
                changeY = 1000;
                break;
            case "helicopterCap":
                changeY = 12;
                break;
            case "jetpack":
                changeY = 20;
                break;
            default:
                break;
        }
        int counter = 50;
        while (counter > 0)
        {
            charLocation.Y += changeY;
            counter--;
        }
        //powerups.RemoveAt(0);
        powerupActivated = false;
    }
}
