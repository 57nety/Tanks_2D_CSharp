using SFML.Graphics;
using SFML.System;
using System;

namespace Tanks_2D_CSharp
{
    class Map
    {
        public class Timer
        {
            private bool activity; // активность таймера в данный момент времени
			public bool Activity { get { return this.activity; } }
            private int initValue; // начальное значение
			public int InitValue { set { this.initValue = value; } }
            private int presentValue; // текущее значение
			public int PresentValue { get { return this.presentValue; } }
            private Clock clock; // часы

			public Timer() // конструктор без параметров класса Timer
			{
				this.presentValue = this.initValue = 0;
				this.activity = false;
			}

			public Timer(int value) // конструктор с одним параметром класса Timer
			{
				this.presentValue = this.initValue = value;
				this.activity = false;
			}

			public void startTimer() // запуск таймера
			{
				if (this.activity == false)
				{
					this.clock.Restart();
					this.activity = true;
				}
				if (this.presentValue > 0)
				{
					this.presentValue = this.initValue - (int)this.clock.ElapsedTime.AsSeconds();
				}
				else
				{
					this.stopTimer();
				}

			}

			// остановка таймера
			public void stopTimer()
			{
				this.activity = false;
				this.initValue = this.presentValue;
			}
		}

		private Timer timerGame; // таймер всей игры
		public Timer TimerGame { get { return this.timerGame; } }
		private Timer timerBonus; // таймер бонуса
		public Timer TimerBonus { get { return this.timerBonus; } }
		private string fileMap; // название файла с картой
		private Image image; // картинка
		private Texture texture; // текстура
		private Sprite sprite; // спрайт
		private int height = 10; // высота карты
		public int Height { get { return this.height; } }
		private int width = 14; // ширина краты
		public int Width { get { return this.width; } }
		private string[] tileMap = new string[10] { // карта
		"wwwwwwwwwwwwww", // w - wall (стена)
		"w            w", // d - double damage (двойной урон)
		"w            w", // s - shield (щит)
		"w            w", // t - upTime (остановка таймера на 20 секунд)
		"w            w",
		"w            w",
		"w            w",
		"w            w",
		"w            w",
		"wwwwwwwwwwwwww",
		};
		public string[] TileMap { get { return this.tileMap; } }
		private bool bonusDoubleDamage; // переменая показывающая, активен ли в данный момент бонус "двойной урон"
		public bool BonusDoubleDamage { get { return this.bonusDoubleDamage; } set { this.bonusDoubleDamage = value; } }
		private bool bonusShield; // переменая показывающая, активен ли в данный момент бонус "щит"
		public bool BonusShield { get { return this.bonusShield; } set { this.bonusShield = value; } }
		private bool bonusUpTime; // переменая показывающая, активен ли в данный момент бонус "остановка таймера на 20 секунд"
		public bool BonusUpTime { get { return this.bonusUpTime; } set { this.bonusUpTime = value; } }

		public Map(string fileMap) // конструктор с одним параметром класса Map
		{
			Timer timerGame = new Timer(120);
			this.timerGame = timerGame;
			this.fileMap = fileMap;
			Image image = new Image("images/" + this.fileMap);
			this.image = image;
			Texture texture = new Texture(this.image);
			this.texture = texture;
			Sprite sprite = new Sprite(this.texture);
			this.sprite = sprite;
			this.bonusDoubleDamage = false;
			this.bonusShield = false;
			this.bonusUpTime = false;
			this.randomMapGenerate();
		}

		public void draw(RenderWindow window) // нарисовать карту
		{
			for (int i = 0; i < this.height; i++)
			{
				for (int j = 0; j < this.width; j++)
				{
					if (this.tileMap[i][j] == ' ')
					{
						IntRect intRect = new IntRect(0, 0, 60, 60);
						this.sprite.TextureRect = intRect;
					}
					else if (this.tileMap[i][j] == 'w')
					{
						IntRect intRect = new IntRect(60, 0, 60, 60);
						this.sprite.TextureRect = intRect;
					}
					else if (this.tileMap[i][j] == 's')
					{
						IntRect intRect = new IntRect(120, 0, 60, 60);
						this.sprite.TextureRect = intRect;
					}
					else if (this.tileMap[i][j] == 'd')
					{
						IntRect intRect = new IntRect(180, 0, 60, 60);
						this.sprite.TextureRect = intRect;
					}
					else if (this.tileMap[i][j] == 't')
					{
						IntRect intRect = new IntRect(240, 0, 60, 60);
						this.sprite.TextureRect = intRect;
					}
					Vector2f vector2F = new Vector2f(j * 60, i * 60);
					this.sprite.Position = vector2F;
					window.Draw(this.sprite);
				}
			}
			this.drawTimer(window);
		}

		public void drawTimer(RenderWindow window) // отобразить таймер на карте
		{
			this.timerGame.startTimer();
			// задание общих параметров текста
			Font font = new Font("other/Arial.ttf");
			Text text = new Text("", font, 40);
			Color color = new Color(0, 0, 0);
			text.FillColor = color;
			text.OutlineColor = color;
			text.Style = Text.Styles.Bold;
			// вывод таймера игры
			Vector2f vector2F = new Vector2f(60, this.width - 5);
			text.Position = vector2F;
			text.DisplayedString = "Game:" + Convert.ToString(this.timerGame.PresentValue);
			window.Draw(text);
			// вывод таймера бонуса
			vector2F.X = this.height * 57;
			vector2F.Y = this.width - 5;
			text.Position = vector2F;
			text.DisplayedString = "Bonus:" + Convert.ToString(this.timerBonus.PresentValue);
			window.Draw(text);
		}

		public void randomMapGenerate() // рандомная генерация объектов на карте
		{

			int randomElementX = 0;
			int randomElementY = 0;
			Random random = new Random(0);

			// размещение на карте двух бонусов "двойной урон"
			int counter = 2;
			while (counter > 0)
			{
				randomElementX = 1 + random.Next() % (this.width - 1);
				randomElementY = 1 + random.Next() % (this.height - 1);

				if (this.tileMap[randomElementY][randomElementX] == ' ')
				{
					this.tileMap.SetValue('d', randomElementY, randomElementX);
					counter--;
				}
			}

			// размещение на карте двух бонусов "щит"
			counter = 2;
			while (counter > 0)
			{
				randomElementX = 1 + random.Next() % (this.width - 1);
				randomElementY = 1 + random.Next() % (this.height - 1);

				if (tileMap[randomElementY][randomElementX] == ' ')
				{
					this.tileMap.SetValue('s', randomElementY, randomElementX);
					counter--;
				}
			}

			// размещение на карте одного бонуса "остановка таймера на 20 секунд"
			counter = 1;
			while (counter > 0)
			{
				randomElementX = 1 + random.Next() % (this.width - 1);
				randomElementY = 1 + random.Next() % (this.height - 1);

				if (tileMap[randomElementY][randomElementX] == ' ')
				{
					this.tileMap.SetValue('t', randomElementY, randomElementX);
					counter--;
				}
			}
		}

		public void printAmountOfHealthHero(int amountOfHealth, RenderWindow window) // печать количества жизней у героя
		{
			Font font = new Font("other/Arial.ttf");
			Text text = new Text("", font, 40);
			Color color = new Color(0, 0, 0);
			text.OutlineColor = color;
			text.FillColor = color;
			text.Style = Text.Styles.Bold;
			Vector2f vector2F = new Vector2f(60, this.width * 39 - 5);
			text.Position = vector2F;
			if (amountOfHealth > 0)
			{
				text.DisplayedString = "Player:" + Convert.ToString(amountOfHealth);
			}
			else
			{
				text.DisplayedString = "Player: 0";
			}
			window.Draw(text);
		}

		public void printAmountOfHealthEnemy(int amountOfHealth, RenderWindow window) // печать количества жизней у врага
		{
			Font font = new Font("other/Arial.ttf");
			Text text = new Text("", font, 40);
			Color color = new Color(0, 0, 0);
			text.OutlineColor = color;
			text.FillColor = color;
			text.Style = Text.Styles.Bold;
			Vector2f vector2F = new Vector2f(this.height * 57, this.width * 39 - 5);
			text.Position = vector2F;
			if (amountOfHealth > 0)
			{
				text.DisplayedString = "Enemy:" + Convert.ToString(amountOfHealth);
			}
			else
			{
				text.DisplayedString = "Enemy: 0";
			}
			window.Draw(text);
		}

		public int getPresentValueTimer() // получить текущее значение таймера
		{
			return this.timerGame.PresentValue;
		}
}
}
