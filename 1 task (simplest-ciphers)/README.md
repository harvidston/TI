# Лабораторная работа №1
Вариант 2: Написать программу, которая выполняет шифрование и дешифрование текстового файла любого размера, содержащего текст на заданном языке, используя следующие алгоритмы шифрования:
- два перестановочных шифра (`метод «железнодорожной изгороди»`(текст на русском языке) и `метод поворачивающейся решетки` (текст на английском языке), использовать размер решетки 4х4 и схему шифрования как в примере на рисунке 1.3 стр.6);
- подстановочный шифр - `алгоритм Виженера`, прогрессивный ключ, текст на русском языке.
Для всех алгоритмов ключ задается с клавиатуры пользователем.
Программа должна игнорировать все символы, не являющиеся буквами заданного алфавита,  и шифровать только текст на заданном языке. Все алгоритмы должны быть реализованы в одной программе. Программа не должна быть написана в консольном режиме. Результат работы программы – зашифрованный/расшифрованный файл/ы.

### Тесты алгоритмов находятся в файле `tests/мои_тесты`