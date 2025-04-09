# DestructionOfObjectsBackend

API для моделирования разрушения объектов в Unity.

## Обзор

Этот API рассчитывает параметры разрушения объектов при столкновениях в Unity. Сервер принимает данные о столкновении от клиента Unity, рассчитывает физические параметры разрушения и возвращает информацию, необходимую для визуализации эффекта.

## Эндпоинты API

- `POST /api/collision` - Обработка столкновения и расчет параметров разрушения
- `GET /api/collision/test` - Проверка работоспособности API

## Структура запроса

```json
{
  "objectId": "string",
  "impactForce": 0,
  "impactPoint": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "impactDirection": {
    "x": 0,
    "y": 0,
    "z": 0
  },
  "objectType": "string",
  "objectMass": 0,
  "objectDimensions": {
    "x": 0,
    "y": 0,
    "z": 0
  }
}
