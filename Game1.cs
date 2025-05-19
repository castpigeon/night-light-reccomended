using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace night_light;



public class Map
{
  //is_flat_world is for superflat, for now always true, 
  // actual world gen to be made later
  //world slice is what each layer is for superflat,
  // ex: grass dirt, dirt,

  public string[,] make_map(int width, int height, string world_slice, bool is_flat_world)
  {
    string[,] map = new string[height, width];
    if (is_flat_world)
    {
      for (int y = 0; y < height; y++)
      {
        for (int x = 0; x < width; x++)
        {
          if (world_slice[y] == '0')
          {
            map[y, x] = "0";
          }
          else
          {
            map[y, x] = "1";
          }
        }
      }
    }
    return map;
  }


  public int[] tile_block(int x, int y, string[,] map)
  {
    int[] t_index = { 0, 0 };
    //if the coordinate in question is not empty space continue, an empty string means no block is there

    if (map[y, x] != "0")
    {

      //checks if the spaces to the right and left of the coordinate are empty
      // checks to make sure it won't be indexing out of bounds for the array first.
      if (x < map.GetLength(0) - 1)
      {

        if (map[y, x + 1] == "0")
        {

          t_index[0] += 1;

        }
      }

      if (x > 0)
      {
        if (map[y, x - 1] == "0")
        {

          t_index[0] += 2;

        }
      }

      //now it does the same thing for the y axis 
      //also checking to make sure that it isn't out of bounds
      if (y > 0)
      {
        if (map[y - 1, x] == "0")
        {

          t_index[1] += 1;
        }
      }
      if (y < map.GetLength(1) - 1)
      {
        if (map[y + 1, x] == "0")
        {

          t_index[0] += 2;

        }
      }
    }


    return t_index;
  }


  public int[,,] tile_full_map(string[,] map)
  {
    int[,,] tiled_textures = new int[map.GetLength(0), map.GetLength(1), 2];
    int[] new_texture;
    Console.WriteLine("width: " + map.GetLength(0).ToString());
    Console.WriteLine("height: " + map.GetLength(1).ToString());
    for (int y = 0; y <= map.GetLength(0) - 1; y++)
    {
      for (int x = 0; x <= map.GetLength(1) - 1; x++)
      {
        Console.WriteLine("x: " + x.ToString());
        Console.WriteLine("y: " + y.ToString());
        new_texture = tile_block(x, y, map);
        tiled_textures[y, x, 0] = new_texture[0];
        tiled_textures[y, x, 1] = new_texture[1];

      }
    }
    return tiled_textures;
  }

  public void draw_map(int[,,] map_textures, SpriteBatch _spriteBatch)
  {

    for (int y = 0; y <= map_textures.GetLength(0) - 1; y++)
    {
      for (int x = 0; x <= map_textures.GetLength(1) - 1; x++)
      {

        _spriteBatch.Draw(block_texture[map_textures[y, x, 1], map_textures[y, x, 0]], new Vector2(x * 50, y * 50), Color.White);
      }
    }
  }


  public Texture2D[,] Texture_slice(Texture2D block_texture)
  {
    Texture2D[,] texture_map = new Texture2D[8, 8];
    return texture_map;
  }
  public string[,] blocks;

  public int[,,] tiled_map;

  public Texture2D[,] block_texture;

}


/*
public class Player
{
  public float x;
  public float y;
  public float width;
  public float height;
  public float x_speed;
  public float y_speed;
  public float max_speed;
  public float accel;
  public string sprite;

  public Player(float new_x, float new_y, float new_width, float new_height, float new_x_speed, float new_y_speed, float new_max_speed, float new_accel, string new_sprite)
  {
    x = new_x;
    y = new_y;
    width = new_width;
    height = new_height;
    x_speed = new_x_speed;
    y_speed = new_y_speed;
    max_speed = new_max_speed;
    accel = new_accel;
    sprite = new_sprite;
  }

  public float[] process_button_board()
  {

    var k_state = Keyboard.GetState();
    float[] direc = { 0, 0 };

    if (k_state.IsKeyDown(Keys.W))
    {
      direc[1] -= 1;
    }
    if (k_state.IsKeyDown(Keys.S))
    {
      direc[1] += 1;
    }
    if (k_state.IsKeyDown(Keys.A))
    {
      direc[0] -= 1;
    }
    if (k_state.IsKeyDown(Keys.D))
    {
      direc[0] += 1;
    }
    return direc;
  }
}

*/

public class Game1 : Game
{
  Map Map_stuff = new Map();


  Player p = new Player(50, 50, 10, 10, 0, 0, 300, 250, "EPIC ASS BALL FUCK YEAH!!!!!!");

  Texture2D playerTexture;
  Texture2D grassTexture;

  private GraphicsDeviceManager _graphics;
  public static SpriteBatch _spriteBatch;

  public Game1()
  {
    _graphics = new GraphicsDeviceManager(this);
    Content.RootDirectory = "Content";
    IsMouseVisible = true;
  }

  protected override void Initialize()
  {
    // TODO: Add your initialization logic here

    base.Initialize();

    Map_stuff.blocks = Map_stuff.make_map(8, 8, "00001111", true);
    Map_stuff.tiled_map = Map_stuff.tile_full_map(Map_stuff.blocks);
  }

  protected override void LoadContent()
  {
    _spriteBatch = new SpriteBatch(GraphicsDevice);

    // TODO: use this.Content to load your game content here

    playerTexture = Content.Load<Texture2D>(p.sprite);
    grassTexture = Content.Load<Texture2D>("grass");
    Map_stuff.block_texture = Map_stuff.Texture_slice(grassTexture);
  }

  protected override void Update(GameTime gameTime)
  {
    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
      Exit();

    // TODO: Add your update logic here 


    float[] direc = p.process_button_board();

    float delta_time = p.accel * (float)gameTime.ElapsedGameTime.TotalSeconds;

    p.x_speed = direc[0] * delta_time;

    p.y_speed = direc[1] * delta_time;

    if (Math.Abs(p.x_speed) > p.max_speed)
    {
      p.x_speed = p.max_speed;
    }
    if (Math.Abs(p.y_speed) > p.max_speed)
    {
      p.y_speed = p.max_speed;
    }
    p.x += p.x_speed;
    p.y += p.y_speed;

    base.Update(gameTime);
  }

  protected override void Draw(GameTime gameTime)
  {


    GraphicsDevice.Clear(Color.CornflowerBlue);

    // TODO: Add your drawing code here
    // to keep in mind:
    // ticks and frames are different things 


    _spriteBatch.Begin();
    _spriteBatch.Draw(playerTexture, new Rectangle((int)p.x, (int)p.y, 20, 20), Color.White);
    //  Map_stuff.draw_map(Map_stuff.tiled_map,_spriteBatch);
    _spriteBatch.End();

    base.Draw(gameTime);
  }
}
