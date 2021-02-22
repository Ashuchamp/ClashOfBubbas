using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*This Character class is responsible for character movements, getting and incrementing the characterLocation,
 dynamically changing the character texture, and character shooting*/
class Character
{
    private Texture charTexture;
    private Vector2 charLocation;
    private int numTexture;

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
    public void setTexture(int num)
    {
        numTexture = num;
        charTexture = Engine.LoadTexture("char" + numTexture + "R.png");
    }
    public int getTextureNum()
    {
        return numTexture;
    }
    public Character()
    {
        charLocation = new Vector2(145, 340);
        numTexture = 1;
        charTexture = Engine.LoadTexture("char" + numTexture + "R.png");
    }
    public void respondToKey(String keyName)
    {
        if (keyName == "A")
        {
            charTexture = Engine.LoadTexture("char" + numTexture + "L.png");

            if (charLocation.X < -5)
            {
                charLocation.X = 300;
            }
            charLocation.X = charLocation.X - 5;
        }
        if (keyName == "D")
        {
            charTexture = Engine.LoadTexture("char" + numTexture + "R.png");

            if (charLocation.X > 305)
            {
                charLocation.X = 0;
            }
            charLocation.X = charLocation.X + 5;
        }
    }
    public Vector2 shoot()
    {
        charTexture = Engine.LoadTexture("char" + numTexture + "Shoot.png");
        Vector2 temp = new Vector2();
        temp = charLocation;
        temp.Y = temp.Y - 2;
        temp.X = temp.X + 15;
        return temp;
    }
}