# [Software Testing docs](README.md) - App Requirements

---

1. As part of the lab sessions, the following types of tests will be practiced

    - Unit tests
    - Integration tests
    - System tests
    - Performance tests
    - Security tests

---

2. General Specification

    - You will be testing an application of your own creation, specifically a shopping list application.
    - The choice of technology stack and testing tools is left to your discretion.
    - The application should be built using the TDD methodology.
    - The assessment will be based on presenting a report of the conducted tests and demonstrating a working application along with running a set of automated tests.

---

3. The conducted tests must be documented in the form of a test plan. The test plan document should include

    - A description of the testing environment and the planned tools to be used for testing.
    - Preparation and description of test cases for unit tests.
    - Preparation and description of test cases for integration tests.
    - Preparation of test data for integration tests, along with an explanation of the reasons for choosing that specific set of data.
    - Preparation and description of test cases for system tests.
    - Preparation of test data for system tests, along with an explanation of the reasons for choosing that specific set of data.
    - Preparation and description of test cases for performance tests.
    - Preparation of a security audit.

---

4. The results of the tests will be collected in a Test Report.
   - It will include the results of the conducted tests and audits.
   - The results must be analyzed, and conclusions drawn in the form of recommendations for the development team.

---

5. Project specifications for the tests
   - create an application (a web application is suggested, including frontend and backend, but any other type of application is acceptable) whose task will be to manage shopping lists.
   - The application will only be accessible to logged-in users.
   - The login and registration process will be carried out by the user independently.
   - After logging in, the user will have the ability to define shopping lists.
   - A suggested purchase date will be assigned to each list.
   - Items will be added to the list in the following way:
       - purchase items (either any item or selected from a list of the most popular items, such as bread, milk, water, cheese, etc.)
       - along with the quantity
       - and indicating the weight (selected from a list of available items).
   - It will be possible to add a photo as a reference element.
   - For each item, there will be an option to mark the purchase as completed, which will be visualized as a crossed-out item on the list.
   - The shopping lists and their current state will be saved in the database.
   - We will handle full CRUD operations for the lists.

---
