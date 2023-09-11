import json

from django.http import HttpResponse
from django.shortcuts import render
from django.views.decorators.csrf import csrf_exempt
from .models import Player

# Create your views here.


@csrf_exempt
def update_player(request):
    data = json.loads(request.body)

    try:
        username = data['username']
        high_score = data['high_score']
        location = data['location']
    except KeyError as e:
        return HttpResponse(e, status=400)

    if Player.objects.filter(name=username).exists():
        player = Player.objects.get(name=username)
        player.high_score = high_score
        player.location = location
        player.save()

    else:
        Player(name=username, high_score=high_score, location=location).save()

    return HttpResponse('Success!', status=200)
