from django.db import models

# Create your models here.


class Player(models.Model):
    name: str = models.CharField(max_length=255)
    high_score: int = models.IntegerField(default=0)
    location: str = models.CharField(max_length=255)

    def __str__(self):
        return self.name
