from django.urls import path
from .views import update_player, get_scores


urlpatterns = [
    path('update', update_player),
    path('players', get_scores),
]
