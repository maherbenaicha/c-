# Exécution rapide (API + Flutter)

## 1) Lancer l’API (FastAPI)

Dans `SignLanguageAI/` :

```bash
python -m venv .venv
.\.venv\Scripts\activate
pip install -r requirements_api.txt
python api_server.py
```

Le serveur écoute sur le port `8000` (par défaut).

## 2) Lancer l’app Flutter

Dans `sign_translate/` :

```bash
flutter pub get
flutter run
```

### Réglage important de l’URL serveur

Dans l’écran **Text to Sign**, ouvre l’icône **⚙ Settings** et mets :

- **Android Emulator**: `http://10.0.2.2:8000`
- **Téléphone (même Wi‑Fi que le PC)**: `http://<IP_LAN_DU_PC>:8000`

