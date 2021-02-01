# Programming Coursework for F21GP 2020/2021

## 1 Overview
This is an individual coursework. The main objective of this programming coursework is to implement a technical prototype using four core components often used in Video Games. For this coursework, you will need to demonstrate and explain your running code. This coursework counts for 40 marks of your overall mark for this course.

## 2 Tools, Libraries and Environment
The following are **required** for your coursework:
- Unity Games Engine (LTS).

The following tools are **recommended** but not required for the development of your programming demo:
- Video Stdio IDE
Unity Probuilder and other add-ons in package manager to create levels
- External assets to create your level and characters (e.g. 3D models, sounds, animations).

The following are **not allowed** for your coursework:
- Both Visual Scripting Package
- A* Pathfinding Project or similar add-ons
- Any Unity add-on or package adding the functionality requested.
- Other games engines (e.g. Unreal Engine, Godot, etc.)

## 3 Task
To complete this coursework, you will need to create a prototype of a **couple** of levels including a game mechanics and the following required components for **both levels**:
1. Objects/characters affected by rigid body physics (inc. reaction to forces and collusions)
2. Crowd interaction between a group of similar objects/characters (at least a dozen)
3. Logic for object/characters including path-planning search (e.g. FSM or Behaviour Trees)
4. Win/lose conditions

**Marking scheme**: The components for your technical demo will be each marked as below:

|  | Below Threshold (F-E) | Threshold (D-B) | Above Threshold (A) |
|-|:-:|:-:|:-:|
| **Min** | Component was not implemented at all. | Component works correctly, but incorrect or incomplete explanation of code or algorithms. | Component runs correctly, code/algorithms explained, and evidence of enhancement of the components. |
| **Max** | Component does not work, or algorithm was not implemented correctly. | Component runs correctly, code and algorithms were explained accurately. | All plus there is interaction between components. |

## 4 Hand-in code

You are **required** to submit a link to a GitHub repository with your code so that it can be checked for plagiarism and for use of external code. This is a requirement for you to pass this coursework. Coursework with no code will receive 0 marks. Please follow the instructions in Vision.

## 5 Technical Demo

You will be asked to demonstrate your prototype (2-3 min) and explain the code in your components (7-8 min) in a recorded video. Please do not make a video longer than 10 minutes.

The deadline for submitting links to your video and code for this coursework is **Thursday 25th of February at 3:30pm**.

If you have any questions or queries about the assessment, please do not hesitate to contact Stefano [S.Padilla@hw.ac.uk](mailto://S.Padilla@hw.ac.uk) (Edinburgh) or Kayvan [kk46@hw.ac.uk](mailto://kk46@hw.ac.uk) (Dubai).

## 6 Late Submissions

The University recognises that, on occasion, students may be unable to submit coursework on the submission date or be unable to present their work on the submission date. In these cases, the University's Submission of Coursework Policy outlines are:
- No individual extensions are permitted under any circumstances.
- Standard 30% deduction from the mark awarded (maximum of five working days).
- In the case where a student submits coursework up to five workings days late, and the student has valid mitigating circumstances, the mitigating circumstances policy will apply, and appropriate mitigation will be applied.
- Any coursework submitted after five calendar days of the set submission date shall be automatically awarded a no grade with no formative feedback provided.

## 7 Notes

- Any borrowed/external code or libraries for the functionality of the components will receive no credits towards your overall mark.
- You must never give hard or soft copies of your coursework code to another student.
- You must always refuse any request from another student for a copy of your code.
- Sharing a coursework code with another student is collusion, and if detected, this will be reported to the School''s Discipline Committee. If found guilty of collusion, the penalty could involving voiding the course [www.hw.acuk/students/doc/plagiarismguide.pdf](www.hw.acuk/students/doc/plagiarismguide.pdf).
- Feedback and your marks: you will receive feedback for this coursework in a maximum 3 teaching weeks from the submission date.

## Marking Sheet

| Marking Scheme Guidelines | Below Threshold (F-E) | Threshold (D-B) | Above Threshold (A) |
|:-:|:-:|:-:|:-:|
| **Rigid Body Physics Component** | **(0.0 - 3.0)**<br/>Object physics does not look correct (e.g. forces). The motion of the object does not settle down. The object falls through the floor/walls. Collusions are incorrect. | **(3.0 - 5.0)**<br/>Component running physics correctly - using forces/velocity vectors. Low boundary: can't explain or point at implementation code. Code confusing. High boundary: code and algorithms explained quite well. Fully functional code. The code structure is clear and understandable. | **(5.0-8.0)**<br/>Component working correctly in both levels. Low boundary: extra walls, multiple bounding objects, different floors, rotational motion, or other. High boundary: interaction with all other components. |
| **Crowd or Flocking Component** | **(0.0 - 3.0)**<br/>The crowd does not look correct. Crowd missing behaviours. Small Crowd (less than 12). | **(3.0 - 5.0)**<br/>Component running crowd simulation implementing 3D behaviours. Low boundary: Can't explain or point at implementation code. Code structure confusing. High boundary: Code and Algorithms explained quite well. Fully functional code. The code structure is clear and understandable. | **(5.0-8.0)**<br/>Component working correctly in both levels. Low boundary: 3D flocking or crowd, extra behaviours, wanders, followers, or other. High boundary: interaction with all other components. |
| **Logic and Search** | **(0.0 - 3.0)**<br/>Character or object does not react. Character foes not reach the goal. No Shown logic. | **(3.0 - 5.0)**<br/>Pathfinding algorithms using correct search space and heuristics. Low boundary: Can't explain or point at implementation code. Code structure confusing. High boundary: Code and Algorithms explained quite well. Fully functional code. The code structure is clear and understandable. | **(5.0-8.0)**<br/>Component working correctly in both levels. Low boundary: boundaries, maze, obstacle, random places to start, extra heuristics improving search and load, or other...  High boundary: interaction with all other components. |
| **Win/Lose Condition** | **(0.0 - 2.5)**<br/>No win/loose conditions. | **(2.5 - 4.0)**<br/>Implemented win and lose conditions. Low boundary: Can't explain or point at implementation code. Code structure confusing. High boundary: Code and Algorithms explained quite well. Fully functional code. The code structure is clear and understandable. | **(4.0 - 6.0)**<br/>Component working correctly in both levels. Low boundary: added score, complex condition. High boundary: interaction with all other components. |
| **Technical Quality of the Prototype** | **(0.0 - 2.0)**<br/>Level with no context. Mismatch assets and ideas. Borrowed whole level from a template. No game mechanic. | **(2.0 - 3.5)**<br/>Level with context, motivation and assets. A game mechanic. Low boundary: context, assets, motivation not explained. High boundary: context, assets, motivation explained. | **(3.5-5.0)**<br/>Low boundary: low level of overall polish. High boundary: high level of overall polish. |
| **Technical Presentation Video** | **(0.0 - 2.0)**<br/>Prototype not shown in full. | **(2.0-3.5)**<br/>Prototype explained in full and code explained for components. | **(3.5-5.0)**<br/>Clear, concise and good quality explanation of code and prototype. |
