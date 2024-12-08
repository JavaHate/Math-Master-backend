import json

def generate_all_questions():
    questions = []
    for i in range(1, 18):
        for j in range(1, 18):
            # Addition
            questions.append({
                "Text": f"{i}+{j}",
                "Answer": i + j,
                "Difficulty": 1
            })
            # Subtraction
            questions.append({
                "Text": f"{i}-{j}",
                "Answer": i - j,
                "Difficulty": 1
            })
            # Multiplication
            questions.append({
                "Text": f"{i}*{j}",
                "Answer": i * j,
                "Difficulty": 1
            })
            # Division (only correct division)
            if i % j == 0:
                questions.append({
                    "Text": f"{i}/{j}",
                    "Answer": i / j,
                    "Difficulty": 1
                })
    return questions

def save_to_json(questions, filename):
    with open(filename, 'w') as f:
        json.dump(questions, f, indent=4)

if __name__ == "__main__":
    questions = generate_all_questions()
    save_to_json(questions, 'questions.json')
    print(f"Generated {len(questions)} questions and saved to 'questions.json'")