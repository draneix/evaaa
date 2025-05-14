
import os
import numpy as np
from mlagents_envs.environment import UnityEnvironment, ActionTuple
from mlagents_envs.side_channel.environment_parameters_channel import EnvironmentParametersChannel
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel

env_file = os.path.join('./envs/two_resource/0.15.10-250509/build')

engineChannel = EngineConfigurationChannel()
paramChannel = EnvironmentParametersChannel()

env = UnityEnvironment(
        file_name=env_file,
        seed=252,
        side_channels=[engineChannel, paramChannel],
        base_port=8210,
        )

engineChannel.set_configuration_parameters(
        time_scale=15,
        width=100,
        height=100,
        )

env.reset()

behavior_name = list(env.behavior_specs)[0]
spec = env.behavior_specs[behavior_name]
n_agents = len(env.get_steps(behavior_name)[0])
branch_sizes = spec.action_spec.discrete_branches
print("spec: ", spec)
print("action_spec: ", branch_sizes)

done = False
for _ in range(1000): 
    random_action = np.column_stack([
        np.random.randint(0, branch_size, size=n_agents)
        for branch_size in branch_sizes
    ])
    action_tuple = ActionTuple()
    action_tuple.add_discrete(random_action)
    env.set_actions(behavior_name, action_tuple)
    env.step()
    decision_steps, terminal_steps = env.get_steps(behavior_name)

    for agent_id in decision_steps:
        print("visual observation: ", decision_steps[agent_id].obs[0].shape)
        print("EV: ", " ".join(map(str, decision_steps[agent_id].obs[1][0:4])))
        print("olfactory: ", " ".join(map(str, decision_steps[agent_id].obs[1][4:14])))
        print("temperature: ", " ".join(map(str, decision_steps[agent_id].obs[1][14:22])))
        print("collision: ", " ".join(map(str, decision_steps[agent_id].obs[1][22:32])))
    
    for agent_id in terminal_steps:
        done = True
        print("*** terminal: ", done)
        env.reset()

env.close()
