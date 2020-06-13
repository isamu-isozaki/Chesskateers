adb -s RF8M909VTTP reverse tcp:8081 tcp:8081
heroku ps:scale web=1
heroku logs --tail

emulator:
adb -s emulator-5554 reverse tcp:8081 tcp:8081