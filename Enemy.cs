using SFML.Graphics;
using SFML.System;

namespace Tanks_2D_CSharp
{
    class Enemy : Tank
    {
		public Enemy(string fileTank, int height, int width, int x, int y) :base(fileTank, height, width, x, y) { } // конструктор

		public void directionOfTravel(Hero hero) // направление движения
		{
			if ((int)(hero.X - this.x) != 0)
			{
				// движение влево
				if ((hero.X - this.x) < 0)
				{
					this.movementToTheLeft();
				}
				// движение вправо
				else if ((hero.X - this.x) > 0)
				{
					this.movementToTheRight();
				}
			}
			else
			{
				if ((int)(hero.Y - this.y) != 0)
				{
					// движение вверх
					if ((hero.Y - this.y) < 0)
					{
						this.movementToTheUp();
					}
					// движение вниз
					else if ((hero.Y - this.y) > 0)
					{
						this.movementToTheDown();
					}
				}
				else
				{
					this.speed = 0;
				}
			}
		}

		public void interactionEnemyWithMap(Map map) // взаимодейтсвие врага с картой
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
				}
			}
		}

		public void update(Map map, Hero hero, RenderWindow window) // обновление состояния танка
		{
			if (this.live == true)
			{
				window.Draw(this.sprite);
				this.directionOfTravel(hero);

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

				float time = this.clock.ElapsedTime.AsMicroseconds();
				this.clock.Restart();
				time = time / 800;

				this.x = this.x + this.dx * time;
				this.y = this.y + this.dy * time;

				Vector2f vector2F = new Vector2f(this.x, this.y);
				this.sprite.Position = vector2F;

				this.interactionOfClassmates(hero);
				map.printAmountOfHealthEnemy(this.amountOfHealth, window);
				this.interactionEnemyWithMap(map);

				if (this.isShoot == false)
				{
					this.shoot();
				}
				if (this.isShoot == true)
				{
					this.bullet.update(time, map, window, this, hero);
				}
			}
		}

		public void movementToTheUp() // движение вверх
		{
			this.direction = 1;
			this.speed = (float)0.03;
			IntRect intRect = new IntRect(0, 0, 40, 40);
			this.sprite.TextureRect = intRect;
		}

		public void movementToTheDown() // движение вниз
		{
			this.direction = 2;
			this.speed = (float)0.03;
			IntRect intRect = new IntRect(0, 40, 40, 40);
			this.sprite.TextureRect = intRect;
		}

		public void movementToTheLeft() // движение влево
		{
			this.direction = 3;
			this.speed = (float)0.03;
			IntRect intRect = new IntRect(40, 80, -40, 40);
			this.sprite.TextureRect = intRect;
		}

		public void movementToTheRight() // движение вправо
		{
			this.direction = 4;
			this.speed = (float)0.03;
			IntRect intRect = new IntRect(0, 80, 40, 40);
			this.sprite.TextureRect = intRect;
		}
	}
}
