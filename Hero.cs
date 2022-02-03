using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace Tanks_2D_CSharp
{
    class Hero : Tank
    {
        public int spriteColumnCoordinate; // координата столбца спрайта

		public Hero(string fileTank, int height, int width, int x, int y) : base(fileTank, height, width, x, y) // конструктор
		{
			this.spriteColumnCoordinate = 0;
		}

		public void directionOfTravel() // направление движения
		{
			// движение вверх
			if (Keyboard.IsKeyPressed(Keyboard.Key.Up) || Keyboard.IsKeyPressed(Keyboard.Key.W))
			{
				this.direction = 1;
				this.speed = (float)0.1;
			}
			// движение вниз
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Down) || Keyboard.IsKeyPressed(Keyboard.Key.S))
			{
				this.direction = 2;
				this.speed = (float)0.1;
			}
			// движение влево
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Left) || Keyboard.IsKeyPressed(Keyboard.Key.A))
			{
				this.direction = 3;
				this.speed = (float)0.1;
			}
			// движение вправо
			else if (Keyboard.IsKeyPressed(Keyboard.Key.Right) || Keyboard.IsKeyPressed(Keyboard.Key.D))
			{
				this.direction = 4;
				this.speed = (float)0.1;
			}
			if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
			{
				if (this.isShoot == false)
				{
					this.shoot();
				}
			}
			this.setSpritePosition();
		}

		public void setSpritePosition() // изменить позицию спрайта в зависимости от направления
		{
			if (this.direction == 1)
			{
				IntRect intRect = new IntRect(this.spriteColumnCoordinate, 0, 40, 40);
				this.sprite.TextureRect = intRect;
			}
			else if (this.direction == 2)
			{
				IntRect intRect = new IntRect(this.spriteColumnCoordinate, 40, 40, 40);
				this.sprite.TextureRect = intRect;
			}
			else if (this.direction == 3)
			{
				IntRect intRect = new IntRect(this.spriteColumnCoordinate + 40, 80, -40, 40);
				this.sprite.TextureRect = intRect;
			}
			else if (this.direction == 4)
			{
				IntRect intRect = new IntRect(this.spriteColumnCoordinate, 80, 40, 40);
				this.sprite.TextureRect = intRect;
			}
		}

		public void interactionHeroWithMap(Map map) // взаимодействие танка с картой
		{
			for (int i = (int)this.y / 60; i < (this.y + this.height) / 60; i++)
			{
				for (int j = (int)this.x / 60; j < (this.x + this.width) / 60; j++)
				{
					// столкновение со стенкой
					if (map.TileMap[i][j] == 'w')
					{
						// движение вниз
						if (this.dy > 0)
						{
							this.y = i * 60 - this.height;
						}
						// движение вверх
						else if (this.dy < 0)
						{
							this.y = i * 60 + 60;
						}
						// движение вправо
						else if (this.dx > 0)
						{
							this.x = j * 60 - this.width;
						}
						// движение влево
						else if (this.dx < 0)
						{
							this.x = j * 60 + 60;
						}
					}
					// проверка того, что в данный момент все бонусы не активны
					if (map.BonusDoubleDamage == false && map.BonusShield == false && map.BonusUpTime == false)
					{
						// столкновение с бонусом "щит"
						if (map.TileMap[i][j] == 's')
						{
							map.TileMap.SetValue(' ', i, j);
							this.spriteColumnCoordinate = 40;
							this.protection = this.protection * 2;
							map.BonusShield = true;
							map.TimerBonus.InitValue = 20;
						}
						// столкновение с бонусом "двойной урон"
						else if (map.TileMap[i][j] == 'd')
						{
							map.TileMap.SetValue(' ', i, j);
							this.spriteColumnCoordinate = 80;
							this.damage = this.damage * 2;
							map.BonusDoubleDamage = true;
							map.TimerBonus.InitValue = 20;
						}
						// столкновение с бонусом "остановка таймера на 20 секунд"
						else if (map.TileMap[i][j] == 't')
						{
							map.TileMap.SetValue(' ', i, j);
							map.BonusUpTime = true;
							map.TimerBonus.InitValue = 20;
						}
					}
				}
			}
			this.checkActivityBonus(map);
		}

		public void checkActivityBonus(Map map) // проверка активности бонуса в данный момент времени
		{
			if (map.BonusShield == true)
			{
				map.TimerBonus.startTimer();
				if (map.TimerBonus.Activity != true)
				{
					this.spriteColumnCoordinate = 0;
					this.protection = this.protection / 2;
					map.BonusShield = false;
				}
			}
			else if (map.BonusDoubleDamage == true)
			{
				map.TimerBonus.startTimer();
				if (map.TimerBonus.Activity != true)
				{
					this.spriteColumnCoordinate = 0;
					this.damage = this.damage / 2;
					map.BonusDoubleDamage = false;
				}
			}
			else if (map.BonusUpTime == true)
			{
				map.TimerGame.stopTimer();
				map.TimerBonus.startTimer();
				if (map.TimerBonus.Activity != true)
				{
					map.TimerGame.startTimer();
					map.BonusUpTime = false;
				}
			}
		}

		public void update(Map map, Enemy enemy, RenderWindow window) // обновление состояния танка
		{
			if (this.live == true)
			{
				window.Draw(this.sprite);

				this.directionOfTravel();

				switch (this.direction)
				{
					case 1: // движение вверх
						this.dx = 0;
						this.dy = -this.speed;
						break;
					case 2: // движение вниз
						this.dx = 0;
						this.dy = this.speed;
						break;
					case 3: // движение влево
						this.dx = -this.speed;
						this.dy = 0;
						break;
					case 4: // движение вправо
						this.dx = this.speed;
						this.dy = 0;
						break;
				}

				this.speed = 0;

				float time = this.clock.ElapsedTime.AsMicroseconds();
				this.clock.Restart();
				time = time / 800;

				this.x = this.x + this.dx * time;
				this.y = this.y + this.dy * time;

				Vector2f vector2F = new Vector2f(this.x, this.y);

				this.interactionHeroWithMap(map);
				map.printAmountOfHealthHero(this.amountOfHealth, window);
				this.interactionOfClassmates(enemy);

				if (this.isShoot == true)
				{
					this.bullet.update(time, map, window, this, enemy);
				}
			}
		}
	}
}
