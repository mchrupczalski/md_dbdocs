# Prerequisites:
- [Node.js & npm](https://www.npmjs.com/get-npm)
  - Download and install
- [sethen/markdown-include](https://github.com/sethen/markdown-include)
  - In Node.js Command Prompt run: 
    - npm install markdown-include -g

# How to Use:
1. Create main readme file which defines general document structure
   1. use: 
      1. **#inlcude "path/to/file.md"** - to merge file into output
      2. **!heading** - at the end of #header, to include header in the table of content
2. Create config.json file for markdown-include with the following content:
    ```
    {
        "build" : "DB_Docs_Info.md",
        "files" : ["ReadMe.md"],
        "tableOfContents": {
            "heading": "# Table of Contents"
        }
    }
    ```
3. Run application and follow prompts in console


# Examples
### _ReadMe.md
```
# PDM Database Documentation


#include "info_files/01_Document_info.md"

---

#include "info_files/02_Schemas.md"

---
```

### include_config.json
```
{
	"build" : "ReadMe.md",
	"files" : ["_ReadMe.md"],
	"tableOfContents": {
		"heading": "# Table of Contents"
	}
}
```