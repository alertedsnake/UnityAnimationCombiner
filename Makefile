VERSION = $(shell awk '/^v[0-9]/ {print $$1; exit }' CHANGELOG.md)
TARGET = AnimationCombiner-$(VERSION).unitypackage

all: build

version:
	@echo $(VERSION)

Assets/AlertedSnake/AnimationCombiner/Editor/Version.cs: CHANGELOG.md
	@sed -i 's/VERSION = ".*"/VERSION = "$(VERSION)"/' $@


$(TARGET): Assets/AlertedSnake/AnimationCombiner/Editor/Version.cs

	# build the unity package
	cup -c 2 -o $@ -s .tmp
	mv .tmp/$@ .
	rm -rf .tmp

	# rebuild the unity package
	unzip -d .tmp $@
	rm $@
	cd .tmp && tar cvf ../$@ * && cd -
	rm -rf .tmp

build: $(TARGET)

clean:
	rm -f $(TARGET)
	rm -rf .tmp
.PHONY: clean
