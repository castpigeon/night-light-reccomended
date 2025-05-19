

namespace night_light;



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
