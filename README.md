# 🚗 FLEXUS - Test Task

Реалізація системи керування та взаємодії з автомобілями з використанням принципів чистої архітектури та Dependency Injection.

## 🛠 Технологічний стек
* **Unity 6** (or latest 2022.3 LTS)
* **Zenject / Extenject:** Dependency Injection framework.
* **Cinemachine:** Procedural camera system.
* **UniTask:** Efficient allocation-free async/await for Unity.
* **Sirenix Odin Inspector:** Advanced inspector customization and serialization.
* **New Input System:** Event-driven input handling.

## 🏗 Архітектура
Проект побудований на базі **MVC** з використанням патерну **Facade** для інкапсуляції логіки. Активно використовується **Data-Driven** підхід. 
Повне інтегрування **Dependency Injection (Zenject)**.

## ✨ Особливості реалізації (Features)
1. **Dynamic Factory:** Спавн автомобілів через ID, що дозволяє легко додавати нові типи машин без зміни коду спавнера.
2. **Visual Feedback:** Інтегрована система підсвітки (Outline), яка реагує на фокус гравця.
3. **Advanced Configuration:** Конфіги машин мають валідацію, кольорове кодування параметрів та графіки потужності прямо в інспекторі.
4. **Context Isolation:** Кожна машина та гравець має свій `GameObjectContext`, що ізолює її залежності від решти світу.

## 🎮 Керування
* **WASD** — Рух гравця / Керування авто.
* **E** — Сісти в машину / Вийти з машини.
* **Mouse** — Огляд камерою.
* **Shift** - Спринт.
*Виконано як тестове завдання.*
