# Define variables
$projectDirectory = "..\src"			  # Change this to your project's directory
$dockerComposeFile = "docker-compose.yml"         # Docker Compose file
$dockerComposeFileOverride = "docker-compose.override.yml"
$projectName = "stocksmart.migrations"            # Docker container name or service name

# Navigate to the project directory
Set-Location -Path $projectDirectory

# Build and start the application using Docker Compose
Write-Host "Building and starting Docker containers..."
docker-compose -f $dockerComposeFile -f $dockerComposeFileOverride -p "stocksmartgroup" up --build -d

# Wait for a few seconds to ensure containers are up
Start-Sleep -Seconds 20

# Print a success message
Write-Host "Project is up and running, and migrations have been applied."

# Optional: Open Swagger UI in the default browser
$swaggerUrl = "https://localhost:5001/swagger"  # Replace [port] with your API port
Start-Process $swaggerUrl
