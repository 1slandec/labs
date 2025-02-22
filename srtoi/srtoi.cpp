#include <iostream>
#include <string>
#include <fstream>
#include <Windows.h>

using namespace std;
#pragma execution_character_set("utf-8") 

// Структура для победителя чемпионата
struct Winner {
    string name;  // Имя
    string surname;   // Фамилия
    string country;    // Страна
    int age;          // Возраст
    string team;      // Команда
    int year;        // Год победы
};

// Структура для индекса
struct Index {
    string key;
    int originalIndex;
};

// --- Задание 1: Массивы с индексами ---

// Ввод данных

int intInput(string msg) {
    int number;
    while (true) {
        cout << msg;
        cin >> number;
        if (cin.fail()) {
            cout << "Вы ввели не целое число. Повторите ввод." << endl;
            cin.clear();
            cin.ignore(10000, '\n');
        }
        else {
            return number;
        }
    }
}

void printWinner(Winner w) {
    cout << "Имя: " << w.name << "\n"
        << "Фамилия: " << w.surname<< "\n"
        << "Страна: " << w.country << "\n"
        << "Возраст: " << w.age << "\n"
        << "Команда: " << w.team << "\n"
        << "Год победы: " << w.year << "\n\n";
}

Winner inputWinner() {
    Winner temp;
    cout << "Введите данные чемпиона" << '\n';
    cout << "Имя: ";
    cin >> temp.name;
    cout << "Фамилия: ";
    cin >> temp.surname;
    cout << "Страна: ";
    cin >> temp.country;
    temp.age = intInput("Возраст: ");
    cout << "Команда: ";
    cin >> temp.team;
    temp.year = intInput("Год победы: ");
    return temp;
}

void inputWinners(Winner* winners, int n) {
    for (int i = 0; i < n; i++) {
        winners[i] = inputWinner();
    }
}

// Создание индекса по возрасту (убывание)
void ageIndexation(Winner* winners, Index* ageIndex, int n) {
    for (int i = 0; i < n; i++) {
        ageIndex[i].key = to_string(winners[i].age);
        ageIndex[i].originalIndex = i;
    }
    // Пузырьковая сортировка по убыванию
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (ageIndex[j].key < ageIndex[j + 1].key) {
                Index temp = ageIndex[j];
                ageIndex[j] = ageIndex[j + 1];
                ageIndex[j + 1] = temp;
            }
        }
    }
}

// Создание индекса по фамилии (возрастание)
void surnameIndexation(Winner* winners, Index* surnameIndex, int n) {
    for (int i = 0; i < n; i++) {
        surnameIndex[i].key = winners[i].surname;
        surnameIndex[i].originalIndex = i;
    }
    // Пузырьковая сортировка по возрастанию
    for (int i = 0; i < n - 1; i++) {
        for (int j = 0; j < n - i - 1; j++) {
            if (surnameIndex[j].key > surnameIndex[j + 1].key) {
                Index temp = surnameIndex[j];
                surnameIndex[j] = surnameIndex[j + 1];
                surnameIndex[j + 1] = temp;
            }
        }
    }
}

// Вывод 
void print1(Winner* winners, Index* index, int n) {
    for (int i = 0; i < n; i++) {
        cout << "Имя: " << winners[index[i].originalIndex].name << "\n"
            << "Фамилия: " << winners[index[i].originalIndex].surname << "\n"
            << "Страна: " << winners[index[i].originalIndex].country << "\n"
            << "Возраст: " << winners[index[i].originalIndex].age << "\n"
            << "Команда: " << winners[index[i].originalIndex].team << "\n"
            << "Год победы: " << winners[index[i].originalIndex].year << "\n\n";
    }
}

// Рекурсивный бинарный поиск по фамилии
int binarySearchSurname(Index* index, string key, int left, int right) {
    if (left > right) return -1;
    int mid = left + (right - left) / 2;
    if (index[mid].key == key) return index[mid].originalIndex;
    if (index[mid].key < key)
        return binarySearchSurname(index, key, mid + 1, right);
    return binarySearchSurname(index, key, left, mid - 1);
}

// Итеративный бинарный поиск по возрасту
int binarySearchAge(Index* index, string key, int n) {
    int left = 0, right = n - 1;
    while (left <= right) {
        int mid = left + (right - left) / 2;
        if (index[mid].key == key) return index[mid].originalIndex;
        if (index[mid].key > key) left = mid + 1;
        else right = mid - 1;
    }
    return -1;
}

// Редактирование записи
void editWinner(Winner* winners, Index* ageIndex, Index* surnameIndex, int index, int n) {
    cout << "Имя: ";
    cin >> winners[index].name;
    cout << "Фамилия: ";
    cin >> winners[index].surname;
    cout << "Страна: ";
    cin >> winners[index].country;
    winners[index].age = intInput("Возраст: ");
    cout << "Команда: ";
    cin >> winners[index].team;
    winners[index].year = intInput("Год победы: ");
    ageIndexation(winners, ageIndex, n);
    surnameIndexation(winners, surnameIndex, n);
}

// Удаление записи по фамилии
void deleteWinner(Winner* winners, Index* ageIndex, Index* surnameIndex, int n, string lastName) {
    int idx = binarySearchSurname(surnameIndex, lastName, 0, n - 1);
    if (idx != -1) {
        for (int i = idx; i < n - 1; i++) {
            winners[i] = winners[i + 1];
        }
        n--;
        ageIndexation(winners, ageIndex, n);
        surnameIndexation(winners, surnameIndex, n);
        cout << "Победитель удалён\n";
    }
    else {
        cout << "Победитель не найден\n";
    }
}

// --- Задание 2: Бинарное дерево ---

struct TreeNode { //Узел бинарного дерева
    string key;       // Фамилия
    int index;
    TreeNode* left;
    TreeNode* right;
};

TreeNode* createNode(string key, int index) { //Создание узла бинарного дерева
    TreeNode* node = new TreeNode;
    node->key = key;
    node->index = index;
    node->left = nullptr;
    node->right = nullptr;
    return node;
}

TreeNode* inputSurnameBT(TreeNode* root, string key, int index) { //Вставка узла
    if (!root) return createNode(key, index);
    if (key < root->key)
        root->left = inputSurnameBT(root->left, key, index);
    else
        root->right = inputSurnameBT(root->right, key, index);
    return root;
}

void outputSurnameBT(TreeNode* root, Winner* winners) { //Вывод по алфавиту c конца (фамилии)
    if (root) {
        outputSurnameBT(root->right, winners);
        cout << "Имя: " << winners[root->index].name << "\n"
            << "Фамилия: " << winners[root->index].surname << "\n"
            << "Страна: " << winners[root->index].country << "\n"
            << "Возраст: " << winners[root->index].age << "\n"
            << "Команда: " << winners[root->index].team << "\n"
            << "Год победы: " << winners[root->index].year << "\n\n";
        outputSurnameBT(root->left, winners);
    }
}

TreeNode* firstSurnameBT(TreeNode* root) { //Нахождение первой фамилии
    while (root->left) root = root->left;
    return root;
}

TreeNode* deleteBT(TreeNode* root, string key) { //Удаление узла по фамилии
    if (!root) return nullptr;
    if (key < root->key)
        root->left = deleteBT(root->left, key);
    else if (key > root->key)
        root->right = deleteBT(root->right, key);
    else {
        if (!root->left) {
            TreeNode* temp = root->right;
            delete root;
            return temp;
        }
        if (!root->right) {
            TreeNode* temp = root->left;
            delete root;
            return temp;
        }
        TreeNode* temp = firstSurnameBT(root->right);
        root->key = temp->key;
        root->index = temp->index;
        root->right = deleteBT(root->right, temp->key);
    }
    return root;
}

void searchSurnameBT(Winner* winners, TreeNode* root, string key) { //Поиск по фамилии
    if (!root) {
        cout << "Чемпион не найден\n";
        return;
    }
    if (root->key == key) {
        printWinner(winners[root->index]);
        return;
    }
    if (key < root->key) {
        searchSurnameBT(winners, root->left, key);
    }
    else {
        searchSurnameBT(winners, root->right, key);
    }
}

// --- Задание 3: Линейный список ---

struct ListNode { //Узел линейного списка
    Winner data;
    ListNode* next;
};

ListNode* createListNode(Winner w) { //Создание узла 
    ListNode* node = new ListNode;
    node->data = w;
    node->next = nullptr;
    return node;
}

void insertSortedList(ListNode*& head, Winner w) { //Вставка узла в отсортированный список на его место
    ListNode* newNode = createListNode(w);
    if (!head || w.surname < head->data.surname) {
        newNode->next = head;
        head = newNode;
        return;
    }
    ListNode* current = head;
    while (current->next && current->next->data.surname < w.surname)
        current = current->next;
    newNode->next = current->next;
    current->next = newNode;
}

void printLinearListByInput(ListNode* head) { //Вывод списка так, ка кон есть
    ListNode* current = head;
    while (current) {
        cout << "Имя: " << current->data.name << "\n"
            << "Фамилия: " << current->data.surname << "\n"
            << "Страна: " << current->data.country << "\n"
            << "Возраст: " << current->data.age << "\n"
            << "Команда: " << current->data.team << "\n"
            << "Год победы: " << current->data.year << "\n\n";
        current = current->next;
    }
}

void printLinearListByAge(ListNode* head, int n) { //Вывод списка, отсортированного по убыванию возраста
    // Копируем данные в массив для сортировки
    Winner* temp = new Winner[n];
    ListNode* current = head;
    int i = 0;
    while (current) {
        temp[i++] = current->data;
        current = current->next;
    }
    // Сортировка
    for (int j = 0; j < n - 1; j++) {
        for (int k = 0; k < n - j - 1; k++) {
            if (temp[k].age < temp[k + 1].age) {
                Winner t = temp[k];
                temp[k] = temp[k + 1];
                temp[k + 1] = t;
            }
        }
    }
    // Вывод
    for (int j = 0; j < n; j++) {
        cout << "Имя: " << temp[j].name << "\n"
            << "Фамилия: " << temp[j].surname << "\n"
            << "Страна: " << temp[j].country << "\n"
            << "Возраст: " << temp[j].age << "\n"
            << "Команда: " << temp[j].team << "\n"
            << "Год победы: " << temp[j].year << "\n\n";
    }
    delete[] temp;
}

void searchSurnameLinear(ListNode* head, string lastName) { //Поиск по фамилии
    ListNode* current = head;
    bool found = false;
    while (current) {
        if (current->data.surname == lastName) {
            cout << "Найдено:\n" << "Имя: " << current->data.name << "\n"
                << "Фамилия: " << current->data.surname << "\n"
                << "Страна: " << current->data.country << "\n"
                << "Возраст: " << current->data.age << "\n"
                << "Команда: " << current->data.team << "\n"
                << "Год победы: " << current->data.year << "\n\n";
            found = true;
        }
        current = current->next;
    }
    if (!found) cout << "Победитель не найден\n";
}

void deleteSurnameLinear(ListNode*& head, string lastName) { //Удаление по фамилии
    if (!head) return;
    if (head->data.surname == lastName) {
        ListNode* temp = head;
        head = head->next;
        delete temp;
        cout << "Победитель удалён\n";
        return;
    }
    ListNode* current = head;
    while (current->next && current->next->data.surname != lastName)
        current = current->next;
    if (current->next) {
        ListNode* temp = current->next;
        current->next = temp->next;
        delete temp;
        cout << "Победитель удалён\n";
    }
    else {
        cout << "Победитель не найден\n";
    }
}

void deleteList(ListNode*& head) { //Очистка памяти
    while (head) {
        ListNode* temp = head;
        head = head->next;
        delete temp;
    }
}

int main() {
    SetConsoleOutputCP(65001);
    SetConsoleCP(65001);
    int a;
    /*cout << "Готовый массив или ввод вручную?" << '\n';
    cin >> a;
    if (a == 1) {
        int n;
        cout << "Введите количество чемпионов: ";
        cin >> n;
        Winner* winners = new Winner[n];
    }*/



    int choice, choice2, idx, idx2, index, n;
    TreeNode* root = nullptr;
    ListNode* head = nullptr;
    string searchAge;
    string searchSurname;
    string key;
    Winner* winners;
    winners = new Winner[5];
    Winner preWinners[5] = {
        {"Данил", "Крышковец", "Россия", 17, "Team Spirit", 2024},
        {"Борис", "Воробьев", "Россия", 21, "Team Spirit", 2024},
        {"Дмитрий", "Соколов", "Россия", 23, "Team Spirit", 2024},
        {"Матье", "Ербуат", "Франция", 24, "Team Vitality", 2023},
        {"Алексей", "Голубев", "Казахстан", 26, "Outsiders", 2022} };
    cout << "Выберите режим работы:\n";
    cout << "1 - Использовать готовый массив\n";
    cout << "2 - Ввести данные вручную\n";
    cin >> choice;
    switch (choice) {
    case 1:
        n = 5;
        winners = new Winner[n];
        for (int i = 0; i < n; i++) {
            winners[i] = preWinners[i];
        }
        break;
    
    case 2:
        n = intInput("Введите количество чемпионов");
        winners = new Winner[n];
        inputWinners(winners, n);
        break;
        }
    Index* ageIndex = new Index[n];
    Index* surnameIndex = new Index[n];

        do {
            choice = intInput("Выберите номер задания:\n1. Работа с индексами\n2. Работа с бинарным деревом\n3. Работа с линейным списком\n0. Выход\n");
            switch (choice) {
            case 1:
                do {
                    ageIndexation(winners, ageIndex, n);
                    surnameIndexation(winners, surnameIndex, n);
                    choice2 = intInput("Выберите номер задания:\n1. Вывод по убыванию возраста\n2. Вывод по фамилии (по алфавиту)\n3. Поиск по фамилии\n4. Поиск по возрасту\n5. Редактирование по индексу\n6. Удаление по фамилии\n0. Выход\n");
                    switch (choice2) {
                        // Задание 1

                        //inputWinners(winners, n);
                    case 1:
                        cout << "\nСортировка по возрасту (по убыванию):\n";
                        print1(winners, ageIndex, n);
                        break;
                    case 2:
                        cout << "\nСортировка по фамилии (по возрастанию):\n";
                        print1(winners, surnameIndex, n);
                        break;
                    case 3:
                        cout << "\nВведите фамилию для поиска: ";
                        cin >> searchSurname;
                        idx = binarySearchSurname(surnameIndex, searchSurname, 0, n - 1);
                        if (idx != -1) printWinner(winners[idx]);
                        else cout << "Не найдено\n";
                        break;
                    case 4:
                        cout << "\nВведите возраст для поиска: ";
                        cin >> searchAge;
                        idx2 = binarySearchAge(ageIndex, searchAge, n);
                        if (idx2 != -1) printWinner(winners[idx2]);
                        else cout << "Не найдено\n";
                        break;
                    case 5:
                        index = intInput("Введите индекс редактируемого элемента");
                        editWinner(winners, ageIndex, surnameIndex, index, n);
                        break;
                    case 6:
                        cout << "Введите фамилию удаляемого игрока:";
                        string surname;
                        cin >> surname;
                        deleteWinner(winners, ageIndex, surnameIndex, n, surname);
                        break;
                    }
                } while (choice2 != 0);
                break;
                // Задание 2
            case 2:
                ageIndexation(winners, ageIndex, n);
                surnameIndexation(winners, surnameIndex, n);
                for (int i = 0; i < n; i++)
                    root = inputSurnameBT(root, winners[i].surname, i);
                do {
                    choice2 = intInput("Выберите номер задания:\n1. Вывод по фамилии с конца\n2. Поиск по фамилии\n3. Удаление по фамилии\n0. Выход\n");
                    cin >> choice2;
                    switch (choice2) {
                    case 1:
                        cout << "\nБинарное дерево по фамилии (по алфавиту с конца):\n";
                        outputSurnameBT(root, winners);
                        break;
                    case 2:
                        cout << "Введите фамилию:\n";
                        cin >> key;
                        searchSurnameBT(winners, root, key);
                        break;
                    case 3:
                        cout << "Введите фамилию:\n";
                        cin >> key;
                        deleteBT(root, key);
                        break;
                    }
                } while (choice2 != 0);
            case 3:
                // Задание 3
                for (int i = 0; i < n; i++)
                    insertSortedList(head, winners[i]);
                do {
                    choice2 = intInput("Выберите номер задания:\n1. Ввод новой записи\n2. Просмотр записей в порядке их ввода\n3. Просмотр записей по убыванию возраста\n4. Поиск по фамилии\n5. Удаление по фамилии\n0. Выход\n");
                    switch (choice2) {
                    case 1:
                        insertSortedList(head, inputWinner());
                        break;
                    case 2:
                        printLinearListByInput(head);
                        break;
                    case 3:
                        printLinearListByAge(head, n);
                        break;
                    case 4:
                        cout << "Введите фамилию:\n";
                        cin >> key;
                        searchSurnameLinear(head, key);
                        break;

                    case 5:
                        cout << "Введите фамилию:\n";
                        cin >> key;
                        deleteSurnameLinear(head, key);
                        break;
                    }
                } while (choice2 != 0);
            }
            break;
        } while (choice != 0);
        // Очистка памяти
        delete[] winners;
        delete[] ageIndex;
        delete[] surnameIndex;
        deleteBT(root, root->key); // Удаление дерева
        deleteList(head);

        return 0;
}