from django.urls import path
from .views import update_player


urlpatterns = [
    path('update', update_player),
]
