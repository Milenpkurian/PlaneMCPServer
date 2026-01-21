# Use .NET 10 base image with Node.js
FROM mcr.microsoft.com/devcontainers/dotnet:10.0-noble

# Install Node.js (if not already included in base image)
RUN apt-get update && apt-get install -y \
    curl \
    git \
    && rm -rf /var/lib/apt/lists/*

# Install Node.js via NVM
RUN curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.0/install.sh | bash && \
    . /root/.nvm/nvm.sh && \
    nvm install node && \
    nvm use node

# Set environment variables
ENV NODE_PATH=/root/.nvm/versions/node/*/lib/node_modules
ENV PATH=/root/.nvm/versions/node/*/bin:$PATH

# Switch to non-root user
USER vscode

# Install global npm packages
RUN npm install -g npm@latest yarn

# Return to root for any final setup
USER root
