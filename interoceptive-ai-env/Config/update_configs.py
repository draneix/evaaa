import os
import json

# Define the base directory for the Config folder
CONFIG_DIR = "/Users/sungwoolee/Documents/InteroceptiveAI/interoceptive-ai-env/Config"
DEFAULT_TEMPLATE_DIR = os.path.join(CONFIG_DIR, "default")

# Editable variables
TEMPLATE_FILE_NAME = "resourceConfig.json"  # Name of the template file
FIELD_NAME = "resourceType"  # Field name to check or add
DEFAULT_VALUE = "Random"  # Default value for the field if it is missing

def get_all_config_files(directory, template_file_name):
    """
    Find all JSON configuration files in the given directory that match the template file name.

    :param directory: Base directory to search for JSON files.
    :param template_file_name: Name of the template file to match.
    :return: List of file paths to JSON files matching the template file name, sorted by file name.
    """
    config_files = []
    for root, _, files in os.walk(directory):
        for file in files:
            if file == template_file_name:  # Match only files with the same name as the template
                config_files.append(os.path.join(root, file))
    # Sort the files by their names
    return sorted(config_files, key=lambda x: os.path.basename(x))

def update_config_files(template, field_name, default_value):
    """
    Update all configuration files to ensure they contain the specified field.

    :param template: Template configuration file path.
    :param field_name: Field name to check or add.
    :param default_value: Default value to add if the field is missing.
    """
    # Load the template configuration
    try:
        with open(template, 'r') as file:
            template_data = json.load(file)
    except Exception as e:
        print(f"Failed to load template file: {e}")
        return

    # Get all configuration files in the directory that match the template file name
    config_files = get_all_config_files(CONFIG_DIR, TEMPLATE_FILE_NAME)

    for config_file in config_files:
        try:
            with open(config_file, 'r') as file:
                data = json.load(file)

            # Check if the field exists in the 'groups' array
            if "groups" in data and isinstance(data["groups"], list):
                for group in data["groups"]:
                    if field_name not in group:
                        print(f"Field '{field_name}' not found in group of {config_file}. Adding default value...")
                        group[field_name] = default_value

            # Print the updated configuration for confirmation
            print(f"Updated configuration for {config_file}:")
            print(json.dumps(data, indent=4))

            # Ask for confirmation before rewriting the file
            confirm = input(f"Do you want to save changes to {config_file}? (yes/no): ").strip().lower()
            if confirm == "yes":
                with open(config_file, 'w') as file:
                    json.dump(data, file, indent=4)
                print(f"Changes saved to {config_file}.")
            else:
                print(f"Changes discarded for {config_file}.")

        except Exception as e:
            print(f"Failed to process {config_file}: {e}")

def main():
    # Construct the full path to the template file
    template_path = os.path.join(DEFAULT_TEMPLATE_DIR, TEMPLATE_FILE_NAME)

    # Call the update function
    update_config_files(template_path, FIELD_NAME, DEFAULT_VALUE)

if __name__ == "__main__":
    main()