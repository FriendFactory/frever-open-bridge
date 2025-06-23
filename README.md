# Frever Bridge

A Unity-based SDK for a [Frever](https://github.com/FriendFactory/frever-open) project. 

It supports two Unity-based applications:
1. [Frever App](https://github.com/FriendFactory/frever-open-unity-client) — a social media platform built with Unity
2. [Asset Manager Tool](https://github.com/FriendFactory/frever-open-asset-manager-tool) — a Unity-based tool for managing and syncing assets

The SDK provides:

* A shared interface layer for backend communication
* Built-in caching logic to optimize performance and reduce redundant network calls
* Internal endpoints used by both clients

## Table Of Contents

- [Getting Started](#getting-started)
- [Dependencies](#dependencies)
  - [Plugins](#plugins)
  - [External Services](#external-services)
- [Structure](#structure)
- [License](#license)
- [Support](#support)
- [Contributing](#contributing)

## Getting Started

**Requirements:**

- Unity version: `2022.3.28f1`

1. Download and install Unity Editor (with iOS and/or Android build support modules)
2. Clone repository
3. Initialize `git lfs` by running `git lfs install` in the project directory, then pull LFS files with `git lfs pull`
4. Open the project from the Unity Hub or Unity Editor
5. Resolve dependencies

*Note: This project has internal and external dependencies that must be properly configured. Ensure the backend services are up and running before testing the application. The project relies on external services connectivity for full functionality. Some features may not work correctly without proper backend configuration and network access to required external services.*

## Dependencies

- up-and-running [Frever Backend](https://github.com/FriendFactory/frever-open-backend)

### Plugins

- [BestHTTP/2](https://assetstore.unity.com/publishers/4137) 2.7.0
- [Best TLS Security](https://assetstore.unity.com/publishers/4137) 2

## License

This project is licensed under the [MIT License](LICENSE).

Please note that the Software may include references to “Frever” and/or “Ixia” and that such terms may be subject to trademark or other intellectual property rights, why it is recommended to remove any such references before distributing the Software.

## Support

This repository is provided as-is, with no active support or maintenance. For inquiries related to the open source project, please contact:

**📧 admin@frever.com**

## Contributing

We welcome forks and reuse! While the platform is no longer maintained by the original team, we hope it serves as a useful resource for:

- Generative media research and tooling
- Mobile-first creative platform development
- AI-enhanced prompt and content flows

Please open issues or pull requests on individual repos if you want to share fixes or improvements.
