using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Tanks_2D_CSharp
{
	class main
	{
		public static void Menu(RenderWindow window)
		{
			{ 
				Sprite menu1 = new Sprite(new Texture(new Image("images/1.png")));
				Sprite menu2 = new Sprite(new Texture(new Image("images/2.png")));
				Sprite menu3 = new Sprite(new Texture(new Image("images/3.png")));
				Sprite about = new Sprite(new Texture(new Image("images/about.png")));

				menu1.Position = new Vector2f(355, 166);
				menu2.Position = new Vector2f(361,279);
				menu3.Position = new Vector2f(356,380);

				bool isMenu = true;
				int menuNum = 0;

				while (isMenu)
				{
					menu1.Color = new Color(129, 181, 221);
					menu2.Color = new Color(129, 181, 221);
					menu3.Color = new Color(129, 181, 221);
					menuNum = 0;
					window.Clear(new Color(129, 181, 221));

					if (new IntRect(355, 166, 141, 40).Contains(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y)) { menu1.Color = new Color(0, 0, 255); menuNum = 1; }
					if (new IntRect(361, 279, 112, 30).Contains(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y)) { menu2.Color = new Color(0, 0, 255); menuNum = 2; }
					if (new IntRect(356, 380, 126, 40).Contains(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y)) { menu3.Color = new Color(0, 0, 255); menuNum = 3; }

					if (Mouse.IsButtonPressed(Mouse.Button.Left))
					{
						if (menuNum == 1) isMenu = false;
						if (menuNum == 2) { window.Draw(about); window.Display(); while (!Keyboard.IsKeyPressed(Keyboard.Key.Escape)) ; }
						if (menuNum == 3) { window.Close(); isMenu = false; }

					}

					window.Draw(menu1);
					window.Draw(menu2);
					window.Draw(menu3);

					window.Display();
				}
			}
		}

		public static void Game(RenderWindow window, ref bool repeate)
        {
			Hero hero = new Hero("hero.png", 40, 40, 61, 500);
			Enemy enemy = new Enemy ("enemy.png", 40, 40, 740, 61);
			Map map = new Map("map.png");
			while (window.IsOpen)
			{
				window.Clear(new Color(255,255,255));

				if (map.getPresentValueTimer() > 0) {
					if (enemy.AmountOfHealth <= 0) {
						if (printResult("You winner!", window)) {
							repeate = true;
						}
					}
					else if (hero.AmountOfHealth <= 0) {
						if (printResult("You lose!", window)) {
							repeate = true;
						}
					}
					else {
						map.draw(window);
						hero.update(map, enemy, window);
						enemy.update(map, hero, window);
					}
				}
				else if (map.getPresentValueTimer() <= 0) {
					if (printResult("There is no winner!", window)) {
						repeate = true;
					}
				}
				window.Display();
			}
			repeate = false;
        }

		static public bool printResult(string value, RenderWindow window)
		{
			window.Clear(new Color(129, 181, 221));
			Text text = new Text("", new Font("other/Arial.ttf"), 80);
			text.OutlineColor = new Color(0, 0, 0);
			text.FillColor = new Color(0, 0, 0);
			text.Style = Text.Styles.Bold;
			text.Position= new Vector2f(100, 175);
			text.DisplayedString = value;
			window.Draw(text);
			Sprite sprite = new Sprite(new Texture("images/ok.png"));
			sprite.Position = new Vector2f(640, 555);
			sprite.Color = new Color(129, 181, 221);
			int menuNum = 0;
			if (new IntRect(640, 555, 55, 32).Contains(Mouse.GetPosition(window).X, Mouse.GetPosition(window).Y)) { sprite.Color = new Color(0,0,255); menuNum = 1; }
			window.Draw(sprite);
			if (Mouse.IsButtonPressed(Mouse.Button.Left) && menuNum == 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		static void Main(string[] args)
		{
			RenderWindow window = new RenderWindow(new VideoMode(840, 600), "Tanks_2D");

			bool repeate = true;
			while (repeate)
			{
				repeate = false;
				Menu(window);
				Game(window,ref repeate);
			}
		}
	}
}
