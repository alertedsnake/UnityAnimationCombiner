VERSION = $(shell awk '/^v[0-9]/ {print $$1; exit }' CHANGELOG.md)
TARGET = AnimationCombiner-$(VERSION).unitypackage
BUILDDIR = .tmp/Assets/AlertedSnake/AnimationCombiner

all: build

version:
	@echo $(VERSION)

Editor/Version.cs: CHANGELOG.md
	@sed -i 's/VERSION = ".*"/VERSION = "$(VERSION)"/' $@


$(TARGET): Editor/Version.cs
	mkdir -p $(BUILDDIR)
	ls | grep -v "Assets" | xargs -i{} cp -a {} $(BUILDDIR)
	.github/workflows/generate_meta.sh 46616ff29ab1c264ebabb2c69a364ea4 .tmp/Assets/AlertedSnake
	.github/workflows/generate_meta.sh 598d5ab811680e94c9025eb0ddeb7c72 .tmp/Assets/AlertedSnake/AnimationCombiner.meta

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


# Thanks to SophieBlueVR for the Makefile skeleton!
