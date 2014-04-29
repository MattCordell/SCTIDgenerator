SCTIDgenerator
==============

Simple library for generating SNOMED CT identifiers
Author: Matt Cordell
License: MIT

The intention of this software is to create a simple library for generating SNOMED CT identifiers.
The primary use for these identifiers are in the creation of extensions.

SNOMED CT is the property of IHTSDO - http://www.ihtsdo.org/
IHTSDO®, SNOMED® and SNOMED CT® are registered trademarks of the International Health Terminology Standards Development Organisation.

Outline:
Following functions must be seeded with a namespace. (To be requested from IHTSDO)
  SCTID.Concept
  SCTID.Description
  SCTID.Relationship

The library will contain a SQLite DB, to track ID allocation. However user will ultimately be responsible for ensuring uniqueness.
  ExportAllocatedIds
  ImportAllocatedIds

Test Demo Application included also
