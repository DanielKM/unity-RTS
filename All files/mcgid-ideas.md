# Ideas of things to change

- Put menu classes into a subfolder of UI

- Flesh out the building controller as a superclass and implement subclasses for buildings

- Create separate logic for "buildings that can train units" and "buildings that can do research"
  - Mixins would be the solution to this type of problem, but C# doesn't exactly support them
  - Extension methods might be able to do what we want, though there may be gremlins here

- Create Entity superclass for buildings and units to support UI changes below

- Create generic UI classes for Panel, ActionPanel, InfoPanel, ProgressPanel, etc
  - Example:
    - InputManager determines that player has selected building
    - InputManager tells UIManager to select entity
    - UIManager tells InfoPanelManager to display entity info
    - UIManager checks if entity is busy in progress (training, researching, etc.)
      - if busy: UIManager tells ProgressPanel to display entity progress
      - if not: UIManager tells ActionPanel to display entity actions
    - each PanelManager asks the entity for the details it needs through the defined interface of the Entity class

- Rename NodeManager to something more semantic, and then consider whether decomposition or restructuring would be beneficial

- Create a class for each type of Research, and declare its cost and prerequisites
  - Then when input/UI/whatever wants a research to happen, the ResearchManager can validate whether the cost and prerequisites are satisfied
