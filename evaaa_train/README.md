[![License: CC BY-SA 4.0](https://img.shields.io/badge/License-CC%20BY--SA%204.0-lightgrey.svg)](https://creativecommons.org/licenses/by-sa/4.0/)
[![Python 3.8+](https://img.shields.io/badge/python-3.8+-blue.svg)](https://www.python.org/downloads/)
[![PyTorch](https://img.shields.io/badge/PyTorch-2.5.0-red.svg)](https://pytorch.org/)

# Modified SheepRL Training Framework

This repository contains a modified version of the [SheepRL](https://github.com/Eclectic-Sheep/sheeprl) training framework, adapted for EVAAA training and evaluation needs. The codebase maintains the core structure of SheepRL while incorporating custom modifications.

## ✨ Features

- 🤖 Three benchmark RL algorithms (DreamerV3, PPO, DQN)
- 🎮 Unity ML-Agents integration
- 📊 Logging and monitoring
- 🎥 Screen and data recording capabilities

## 📄 Table of Contents
- [Implemented Algorithms](#implemented-algorithms)
- [Project Structure](#project-structure)
- [Quick Start](#quick-start)
- [Minimal Environment Interaction](#minimal-environment-interaction)
- [Requirements](#requirements)
- [Usage](#usage)
- [Configuration](#configuration)
- [Logging](#logging)
- [Environment Setup](#environment-setup)
- [Troubleshooting](#troubleshooting)
- [Contributing](#contributing)
- [Citation](#citation)
- [License](#license)

## 🧮 Implemented Algorithms

This repository includes three main algorithms:
- **DreamerV3**: Implemented from SheepRL framework
- **PPO (Proximal Policy Optimization)**: Implemented from SheepRL framework
- **DQN (Deep Q-Network)**: Custom implementation by our team

## 📁 Project Structure

```
.
├── algos/           # Algorithm implementations
│   ├── dreamer_v3/  # DreamerV3 from SheepRL
│   ├── ppo/        # PPO from SheepRL
│   └── dqn/        # Custom DQN implementation
├── configs/         # Configuration files
├── envs/           # Environment definitions
├── models/         # Neural network models
├── utils/          # Utility functions
├── train.py        # Training script
├── eval.py         # Evaluation script
├── cli.py          # Command-line interface
├── command.sh      # Example commands
└── simple_example.py # Basic environment interaction example
```

## 🚀 Quick Start

```bash
# Clone the repository
git clone [repository-url]

# Install dependencies
pip install -r requirements.txt

# Download and setup the environment
pip install gdown
gdown --fuzzy "https://drive.google.com/file/d/14pgW30OrynErDS_6BrGjbAc91iS35oL-/view?usp=sharing"
unzip evaaa.zip -d envs/
rm evaaa.zip

# Run a simple example
python simple_example.py

# Start training
python train.py exp=dqn tag=dqn_tet seed=252 env.port=8210 env.time_scale=15 env.width=100 env.height=100 env.config=train-level-1.2-CorneredResource
```

## 🎮 Minimal Environment Interaction

The EVAAA uses Unity ML-Agents for environment interaction. For detailed documentation about the ML-Agents API, please refer to the [official ML-Agents documentation](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Python-LLAPI.md). For a complete example with environment interaction, including all observation types (visual, essential variables, olfactory, temperature, collision) and proper action handling, please refer to [`simple_example.py`](./simple_example.py). Here's a minimal example showing the basic structure:

```python
import os
from mlagents_envs.environment import UnityEnvironment, ActionTuple
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

# Initialize environment
env = UnityEnvironment(
    file_name='./envs/two_resource/0.15.10-250509/build',
    seed=252,
    base_port=8210,
)

# Configure environment
env.reset()
behavior_name = list(env.behavior_specs)[0]

# Simple interaction loop
for _ in range(10): 
    # Execute random action
    action = ActionTuple()
    action.add_discrete([[0]])  # Example action
    env.set_actions(behavior_name, action)
    env.step()
    
    # Get observations
    decision_steps, terminal_steps = env.get_steps(behavior_name)
    for agent_id in decision_steps:
        obs = decision_steps[agent_id].obs
        print(f"Visual shape: {obs[0].shape}")
        print(f"State vector: {obs[1]}")

env.close()
```

This example demonstrates the basic interaction with the environment using ML-Agents, including:
- Environment initialization
- Action execution
- Observation retrieval

## 🔧 Requirements

The project requires the following dependencies:
```
mlagents_envs==1.1.0
numpy==1.23.5
torch==2.5.0
hydra-core==1.3.0
lightning==2.5.0
tensorboard==2.18.0
rich==13.9.4
gymnasium==1.0.0
opencv-python==4.10.0.84
```

Install the dependencies using pip:
```bash
pip install -r requirements.txt
```

## 🚀 Usage

### Training

<details>
<summary>DQN Training Command</summary>

```bash
python train.py exp=dqn tag=dqn_tet seed=252 env.port=8210 env.time_scale=15 env.width=100 env.height=100 env.config=train-level-1.2-CorneredResource env.screenRecordEnable=false env.dataRecordEnable=false
```
</details>

<details>
<summary>PPO Training Command</summary>

```bash
python train.py exp=ppo tag=ppo_tet seed=252 env.port=8210 env.time_scale=15 env.width=100 env.height=100 env.config=train-level-1.2-CorneredResource env.screenRecordEnable=false env.dataRecordEnable=false
```
</details>

<details>
<summary>DreamerV3 Training Command</summary>

```bash
python train.py exp=dreamer_v3 tag=dreamer_tet seed=252 env.port=8210 env.time_scale=15 env.width=100 env.height=100 env.config=train-level-1.2-CorneredResource env.screenRecordEnable=false env.dataRecordEnable=false
```
</details>

### Parameters

| Parameter | Type | Description | Default |
|-----------|------|-------------|---------|
| `exp` | string | Experiment type (dqn, ppo, or dreamer_v3) | - |
| `tag` | string | Experiment tag for identification | - |
| `seed` | int | Random seed for reproducibility | 252 |
| `env.port` | int | Environment port number | 8210 |
| `env.time_scale` | int | Time scale for environment simulation | 15 |
| `env.width` | int | Environment width | 100 |
| `env.height` | int | Environment height | 100 |
| `env.config` | string | Environment configuration file | - |
| `env.screenRecordEnable` | bool | Enable/disable screen recording | false |
| `env.dataRecordEnable` | bool | Enable/disable data recording | false |

### Evaluation

To evaluate a trained model, use the `eval.py` script:

```bash
python eval.py exp=dqn tag=dqn_tet seed=252 env.port=8210 env.time_scale=1 env.width=1000 env.height=1000 env.config=exp-damage env.screenRecordEnable=false env.dataRecordEnable=false checkpoint_path=/path/to/checkpoint
```

Additional parameters for evaluation:
- `checkpoint_path`: Path to the model checkpoint file

## ⚙️ Configuration

The project uses Hydra for configuration management. Configuration files are stored in the `configs/` directory. You can modify these files to adjust:
- Training parameters
- Model architecture
- Environment settings
- Logging configuration
- Hardware settings (CUDA, CPU threads, etc.)

## 📊 Logging

Training logs and checkpoints are saved in the `logs/` directory, organized by experiment name and timestamp. The logging system includes:
- Training metrics
- Environment configurations
- Model checkpoints

## 🎮 Environment Setup

The framework supports custom environment configurations through the `envs/` directory. Each environment can have its own:
- Configuration files
- Build files
- Data / Screen recording 

### Recording Outputs

When running experiments with recording enabled:
- Screen recordings (`env.screenRecordEnable=true`): Saved in `envs/[environment_name]/[version]/Recordings/`
- Data recordings (`env.dataRecordEnable=true`): Saved in `envs/[environment_name]/[version]/Data/`

Example paths for the two_resource environment:
- Screen recordings: `envs/two_resource/0.15.10-250509/Recordings/`
- Data recordings: `envs/two_resource/0.15.10-250509/Data/`

## 📥 Environment Setup

The environment files are too large to be included in the repository directly. Follow these steps to download and set up the environment:

1. Install gdown:
```bash
pip install gdown
```

2. Download the environment files:
```bash
gdown --fuzzy "https://drive.google.com/file/d/1kyySDFI2Bsrk4xeLnWZJCGFepEbpGPsB/view?usp=drive_link"
```

3. Extract and move the files:
```bash
unzip linux.zip -d evaaa
mv evaaa envs/
```

After completing these steps, the environment will be ready to use with the training and evaluation scripts.

## 🔍 Troubleshooting

Common issues and their solutions:

- **Issue**: Environment connection failed
  **Solution**: Check if the port is available


## 📚 Citation

If you use this code in your research, please cite our paper:

```bibtex
@article{your-paper,
  title={Your Paper Title},
  author={Your Name and Co-authors},
  journal={NeurIPS},
  year={2025}
}
```

## 📄 License
CC BY-SA 4.0