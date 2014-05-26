instavote.open
==============

Kurzfeedback geben zu Trainings und mehr. Ein Beispiel für Flow-Design in Web-Anwendungen.

Ein Projekt, in dem ich ausprobieren wollte, wie sich Dialoge in Web-Anwendungen an eine Domäne mit Flow-Design binden lassen.

Zum Einatz gekommen sind dabei NancyFx und MongoDB.

Das Ergebnis ist eine kleine Anwendung, mit der Trainer in ihren Seminaren Feedback einholen können.

1. Trainer registrieren ein Seminar mit einem Kürzel (Matchcode) unter _/trainings/trainerkürzel_, z.B. _/trainings/peterm_.
2. Teilnehmer rufen am Anfang eines Seminars die Root / auf, tragen ein Seminarkürzel ein und registrieren sich mit Name und Email-Adresse.
3. Teilnehmer rufen am Ende eines Seminars die Root / auf, tragen das Seminarkürzel ein und geben unter ihrer Email-Adresse Feedback. Das Feedback besteht aus einer Bewertung und Vorschlägen zur Verbesserung.

Vielen Dank an Daniel Kreiseder für seine Hilfe bei der Gestaltung der HTML-Dialoge.

Enjoy!

-Ralf Westphal