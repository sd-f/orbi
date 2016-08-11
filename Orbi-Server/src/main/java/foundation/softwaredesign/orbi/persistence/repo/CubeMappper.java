package foundation.softwaredesign.orbi.persistence.repo;

import foundation.softwaredesign.orbi.model.virtual.Coordinates;
import foundation.softwaredesign.orbi.model.virtual.Cube;
import foundation.softwaredesign.orbi.persistence.entity.CubeEntity;
import org.apache.deltaspike.data.api.mapping.SimpleQueryInOutMapperBase;

import static java.util.Objects.isNull;
import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class CubeMappper extends SimpleQueryInOutMapperBase<CubeEntity, Cube> {

    @Override
    protected Object getPrimaryKey(Cube cube) {
        return cube.getId();
    }

    @Override
    protected Cube toDto(CubeEntity cubeEntity) {
        Cube cube = new Cube();
        cube.setId(cubeEntity.getId());
        cube.setCoordinates(new Coordinates());
        cube.getCoordinates().setX(cubeEntity.getLongitude());
        cube.getCoordinates().setY(cubeEntity.getElevation());
        cube.getCoordinates().setZ(cubeEntity.getLatitude());
        return cube;
    }

    @Override
    protected CubeEntity toEntity(CubeEntity cubeEntity, Cube cube) {
        CubeEntity newCubeEntity = cubeEntity;
        if (isNull(newCubeEntity)) {
            newCubeEntity = new CubeEntity();
        }
        if (nonNull(cube.getCoordinates())) {
            newCubeEntity.setLongitude(cube.getCoordinates().getX());
            newCubeEntity.setElevation(cube.getCoordinates().getY());
            newCubeEntity.setLatitude(cube.getCoordinates().getZ());
        }
        return newCubeEntity;
    }
}
