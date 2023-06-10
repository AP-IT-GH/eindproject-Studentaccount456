Environment script hint we may need to use later:

The BehaviorParameters component is useful in the environment script because it allows you to customize various settings for the agents in your environment. Here are some reasons why you might want to use the BehaviorParameters component:

Number of agents: You can use the BehaviorParameters component to specify the number of agents in your environment. This can be useful if you want to create an environment with multiple agents that interact with each other.
Observation space: You can use the BehaviorParameters component to specify the size and type of the observation space for your agents. This can be useful if you want your agents to receive different types of observations, such as visual or numerical inputs.
Action space: You can use the BehaviorParameters component to specify the size and type of the action space for your agents. This can be useful if you want your agents to take different types of actions, such as discrete or continuous actions.
Training settings: You can use the BehaviorParameters component to specify various training settings for your agents, such as the maximum number of steps per episode, the number of episodes to train for, and the learning rate of the neural network.
By using the BehaviorParameters component, you can easily customize your environment to meet your needs and experiment with different settings to see how they affect the behavior of your agents.

For more information on how to use the BehaviorParameters component, you can refer to the Unity ML-Agents documentation and the ML-Agents GitHub repository.

Agentscript onactionreceived:
If the distance is greater than or equal to the threshold, we give the agent a small negative reward of -0.01. This encourages the agent to move towards the target, but not necessarily to catch it immediately.
