package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.model.real.Position;
import foundation.softwaredesign.orbi.model.virtual.Cube;
import foundation.softwaredesign.orbi.model.virtual.World;

import javax.enterprise.context.ApplicationScoped;

import java.math.BigDecimal;

import static java.util.Objects.nonNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@ApplicationScoped
public class WorldAdapter {

    private Boolean isWorldOk(World world) {
        if (nonNull(world) && nonNull(world.getCubes()) && !world.getCubes().isEmpty()) {
            return true;
        }
        return false;
    }

    private Boolean isPositionOk(Position position) {
        if (nonNull(position)) {
            if (nonNull(position.getElevation())
                    && nonNull(position.getLatitude())
                && nonNull(position.getLongitute())) {
                return true;
            }
        }
        return false;
    }

    private Boolean isCubeOk(Cube cube) {
        if (nonNull(cube) && nonNull(cube.getCoordinates())) {
            if (nonNull(cube.getCoordinates().getX())
                    && nonNull(cube.getCoordinates().getY())
                    && nonNull(cube.getCoordinates().getZ())) {
                return true;
            }
        }
        return false;
    }

    public void convertToVirtual(World world, Position position) {
        if (isWorldOk(world) && isPositionOk(position)) {
            world.getCubes()
                    .stream()
                    .filter(this::isCubeOk)
                    .forEach(cube -> {
                        BigDecimal newX = cube.getCoordinates().getX().subtract(position.getLongitute());
                        newX = newX.multiply(new BigDecimal(50000));
                        cube.getCoordinates().setX(newX);
                        BigDecimal newY = cube.getCoordinates().getY().subtract(position.getElevation());
                        newY = newY.multiply(new BigDecimal(50000));
                        cube.getCoordinates().setY(newY);
                        BigDecimal newZ = cube.getCoordinates().getZ().subtract(position.getLatitude());
                        newZ = newZ.multiply(new BigDecimal(50000));
                        cube.getCoordinates().setZ(newZ);
            });
        }
    }
}
